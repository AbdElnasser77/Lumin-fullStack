using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using server.Application.Common.Exceptions;
using server.Application.Common.Settings;
using server.Application.DTOs.Auth;
using server.Application.Interfaces;
using server.Domain.Entities;
using server.Domain.Interfaces;

namespace server.Application.Services;

public class AuthService : IAuthService
{
    private const int OtpLifetimeMinutes = 10;

    private readonly IUserRepository _users;
    private readonly IEmailVerificationRepository _emailVerifications;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtTokenService _jwt;
    private readonly IEmailSender _email;
    private readonly JwtOptions _jwtOptions;

    public AuthService(
        IUserRepository users,
        IEmailVerificationRepository emailVerifications,
        IPasswordHasher hasher,
        IJwtTokenService jwt,
        IEmailSender email,
        IOptions<JwtOptions> jwtOptions)
    {
        _users = users;
        _emailVerifications = emailVerifications;
        _hasher = hasher;
        _jwt = jwt;
        _email = email;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<UserResponse> SignUpAsync(SignUpRequest request, CancellationToken ct = default)
    {
        if (!request.CheckTerms)
            throw new AuthException("You must accept the terms and conditions.", 400);

        var email = NormalizeEmail(request.Email);
        var existingUser = await _users.GetByEmailAsync(email, ct);

        if (existingUser is not null)
        {
            if (existingUser.IsEmailVerified)
                throw new AuthException("An account with this email already exists.", 409);

            // Unverified account — overwrite with new registration data and resend OTP
            existingUser.FirstName = request.FirstName.Trim();
            existingUser.LastName = request.LastName.Trim();
            existingUser.PasswordHash = _hasher.Hash(request.Password);
            existingUser.CheckTerms = true;
            existingUser.UpdatedAt = DateTime.UtcNow;
            _users.Update(existingUser);

            var otp = CreateEmailVerification(existingUser.Id, out var ev);
            await _emailVerifications.AddAsync(ev, ct);
            await _users.SaveChangesAsync(ct);

            await _email.SendAsync(
                existingUser.Email,
                "Welcome — confirm your email",
                $"Your verification code is {otp}. It expires in {OtpLifetimeMinutes} minutes.",
                ct);

            return ToUserResponse(existingUser);
        }

        var user = new User
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = email,
            PasswordHash = _hasher.Hash(request.Password),
            CheckTerms = true,
            IsEmailVerified = false
        };

        await _users.AddAsync(user, ct);

        var newOtp = CreateEmailVerification(user.Id, out var newEv);
        await _emailVerifications.AddAsync(newEv, ct);
        await _users.SaveChangesAsync(ct);

        await _email.SendAsync(
            user.Email,
            "Welcome — confirm your email",
            $"Your verification code is {newOtp}. It expires in {OtpLifetimeMinutes} minutes.",
            ct);

        return ToUserResponse(user);
    }

    public async Task<AuthResponse> SignInAsync(SignInRequest request, CancellationToken ct = default)
    {
        var email = NormalizeEmail(request.Email);
        var user = await _users.GetByEmailAsync(email, ct)
            ?? throw new AuthException("Invalid email or password.", 401);

        if (string.IsNullOrEmpty(user.PasswordHash) || !_hasher.Verify(request.Password, user.PasswordHash))
            throw new AuthException("Invalid email or password.", 401);

        if (!user.IsEmailVerified)
            throw new AuthException("Please verify your email address before signing in.", 403);

        var (accessToken, accessExpiry) = _jwt.GenerateToken(user);
        var (refreshToken, refreshExpiry) = IssueRefreshToken(user);

        user.UpdatedAt = DateTime.UtcNow;
        _users.Update(user);
        await _users.SaveChangesAsync(ct);

        return BuildAuthResponse(accessToken, accessExpiry, refreshToken, refreshExpiry, user);
    }

    public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken ct = default)
    {
        var email = NormalizeEmail(request.Email);
        var user = await _users.GetByEmailAsync(email, ct);

        const string genericMessage = "If an account exists for this email, a password reset link has been sent.";

        if (user is null)
            return new ForgotPasswordResponse { Message = genericMessage };

        var (resetToken, _) = IssueResetToken(user);
        _users.Update(user);
        await _users.SaveChangesAsync(ct);

        var link = $"{request.RedirectUrl.TrimEnd('/')}?token={Uri.EscapeDataString(resetToken)}";
        await _email.SendAsync(
            user.Email,
            "Reset your password",
            $"Click the link below to reset your password. It expires in {OtpLifetimeMinutes} minutes.\n\n{link}",
            ct);

        return new ForgotPasswordResponse { Message = genericMessage, ResetToken = resetToken };
    }

    public async Task SendEmailConfirmationAsync(SendEmailConfirmationRequest request, CancellationToken ct = default)
    {
        var email = NormalizeEmail(request.Email);
        var user = await _users.GetByEmailAsync(email, ct);
        if (user is null || user.IsEmailVerified)
            return;

        var otp = CreateEmailVerification(user.Id, out var ev);
        await _emailVerifications.AddAsync(ev, ct);
        await _emailVerifications.SaveChangesAsync(ct);

        await _email.SendAsync(
            user.Email,
            "Confirm your email",
            $"Your confirmation code is {otp}. It expires in {OtpLifetimeMinutes} minutes.",
            ct);
    }

    public async Task<AuthResponse> ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken ct = default)
    {
        var email = NormalizeEmail(request.Email);
        var user = await _users.GetByEmailAsync(email, ct)
            ?? throw new AuthException("Invalid or expired code.", 400);

        var ev = await _emailVerifications.GetLatestByUserIdAsync(user.Id, ct)
            ?? throw new AuthException("Invalid or expired code.", 400);

        if (ev.VerifiedAt is not null || ev.ExpiresAt < DateTime.UtcNow || !_hasher.Verify(request.Otp, ev.OtpHash))
            throw new AuthException("Invalid or expired code.", 400);

        ev.VerifiedAt = DateTime.UtcNow;
        user.IsEmailVerified = true;
        user.UpdatedAt = DateTime.UtcNow;

        var (accessToken, accessExpiry) = _jwt.GenerateToken(user);
        var (refreshToken, refreshExpiry) = IssueRefreshToken(user);

        _emailVerifications.Update(ev);
        _users.Update(user);
        await _users.SaveChangesAsync(ct);

        return BuildAuthResponse(accessToken, accessExpiry, refreshToken, refreshExpiry, user);
    }

    public async Task ResetPasswordAsync(ResetPasswordRequest request, CancellationToken ct = default)
    {
        var dot = request.Token.IndexOf('.');
        if (dot < 0 || !Guid.TryParse(request.Token[..dot], out var userId))
            throw new AuthException("Invalid or expired reset token.", 400);

        var user = await _users.GetByIdAsync(userId, ct)
            ?? throw new AuthException("Invalid or expired reset token.", 400);

        if (string.IsNullOrEmpty(user.OtpHash) || user.OtpExpiresAt < DateTime.UtcNow)
            throw new AuthException("Invalid or expired reset token.", 400);

        var incoming = SHA256.HashData(Encoding.UTF8.GetBytes(request.Token));
        var stored = Convert.FromBase64String(user.OtpHash);
        if (!CryptographicOperations.FixedTimeEquals(incoming, stored))
            throw new AuthException("Invalid or expired reset token.", 400);

        user.PasswordHash = _hasher.Hash(request.NewPassword);
        user.IsEmailVerified = true;
        user.OtpHash = null;
        user.OtpExpiresAt = null;
        user.OtpVerifiedAt = null;
        user.UpdatedAt = DateTime.UtcNow;

        _users.Update(user);
        await _users.SaveChangesAsync(ct);
    }

    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct = default)
    {
        var dot = request.RefreshToken.IndexOf('.');
        if (dot < 0 || !Guid.TryParse(request.RefreshToken[..dot], out var userId))
            throw new AuthException("Invalid refresh token.", 401);

        var user = await _users.GetByIdAsync(userId, ct)
            ?? throw new AuthException("Invalid refresh token.", 401);

        if (string.IsNullOrEmpty(user.RefreshTokenHash) || user.RefreshTokenExpiresAt < DateTime.UtcNow)
            throw new AuthException("Refresh token expired.", 401);

        var incoming = SHA256.HashData(Encoding.UTF8.GetBytes(request.RefreshToken));
        var stored = Convert.FromBase64String(user.RefreshTokenHash);
        if (!CryptographicOperations.FixedTimeEquals(incoming, stored))
            throw new AuthException("Invalid refresh token.", 401);

        var (accessToken, accessExpiry) = _jwt.GenerateToken(user);
        var (refreshToken, refreshExpiry) = IssueRefreshToken(user);

        user.UpdatedAt = DateTime.UtcNow;
        _users.Update(user);
        await _users.SaveChangesAsync(ct);

        return BuildAuthResponse(accessToken, accessExpiry, refreshToken, refreshExpiry, user);
    }

    // ── private helpers ──────────────────────────────────────────────────────

    private (string token, DateTime expiresAt) IssueRefreshToken(User user)
    {
        var random = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        var token = $"{user.Id}.{random}";
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(token));

        user.RefreshTokenHash = Convert.ToBase64String(hash);
        user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpiryDays);

        return (token, user.RefreshTokenExpiresAt.Value);
    }

    private (string token, DateTime expiresAt) IssueResetToken(User user)
    {
        var random = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        var token = $"{user.Id}.{random}";
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(token));

        user.OtpHash = Convert.ToBase64String(hash);
        user.OtpExpiresAt = DateTime.UtcNow.AddMinutes(OtpLifetimeMinutes);
        user.OtpVerifiedAt = null;
        user.UpdatedAt = DateTime.UtcNow;

        return (token, user.OtpExpiresAt.Value);
    }

    private string CreateEmailVerification(Guid userId, out EmailVerification ev)
    {
        var otp = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");
        ev = new EmailVerification
        {
            UserId = userId,
            OtpHash = _hasher.Hash(otp),
            ExpiresAt = DateTime.UtcNow.AddMinutes(OtpLifetimeMinutes)
        };
        return otp;
    }

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();

    private static UserResponse ToUserResponse(User user) => new()
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email,
        IsEmailVerified = user.IsEmailVerified
    };

    private static AuthResponse BuildAuthResponse(
        string accessToken, DateTime accessExpiry,
        string refreshToken, DateTime refreshExpiry,
        User user) => new()
    {
        Token = accessToken,
        ExpiresAt = accessExpiry,
        RefreshToken = refreshToken,
        RefreshTokenExpiresAt = refreshExpiry,
        User = ToUserResponse(user)
    };
}
