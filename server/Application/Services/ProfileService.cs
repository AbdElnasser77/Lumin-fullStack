using System.Security.Cryptography;
using server.Application.Common.Exceptions;
using server.Application.DTOs.Auth;
using server.Application.DTOs.Profile;
using server.Application.Interfaces;
using server.Domain.Interfaces;

namespace server.Application.Services;

public class ProfileService : IProfileService
{
    private const int OtpLifetimeMinutes = 10;

    private readonly IUserRepository _users;
    private readonly IPasswordHasher _hasher;
    private readonly IEmailSender _email;

    public ProfileService(IUserRepository users, IPasswordHasher hasher, IEmailSender email)
    {
        _users = users;
        _hasher = hasher;
        _email = email;
    }

    public async Task<UserResponse> GetProfileAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await _users.GetByIdAsync(userId, ct)
            ?? throw new AuthException("User not found.", 404);

        return ToUserResponse(user);
    }

    public async Task UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken ct = default)
    {
        var user = await _users.GetByIdAsync(userId, ct)
            ?? throw new AuthException("User not found.", 404);

        user.FirstName = request.FirstName.Trim();
        user.LastName = request.LastName.Trim();
        user.UpdatedAt = DateTime.UtcNow;

        _users.Update(user);
        await _users.SaveChangesAsync(ct);
    }

    public async Task ChangePasswordAsync(Guid userId, ChangePasswordRequest request, CancellationToken ct = default)
    {
        var user = await _users.GetByIdAsync(userId, ct)
            ?? throw new AuthException("User not found.", 404);

        if (string.IsNullOrEmpty(user.PasswordHash) || !_hasher.Verify(request.CurrentPassword, user.PasswordHash))
            throw new AuthException("Current password is incorrect.", 400);

        user.PasswordHash = _hasher.Hash(request.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;

        _users.Update(user);
        await _users.SaveChangesAsync(ct);
    }

    public async Task RequestEmailChangeAsync(Guid userId, RequestEmailChangeRequest request, CancellationToken ct = default)
    {
        var user = await _users.GetByIdAsync(userId, ct)
            ?? throw new AuthException("User not found.", 404);

        var newEmail = request.NewEmail.Trim().ToLowerInvariant();

        if (await _users.ExistsByEmailAsync(newEmail, ct))
            throw new AuthException("Email is already in use.", 409);

        var otp = GenerateOtp(out var otpHash);
        user.PendingEmail = newEmail;
        user.EmailChangeOtpHash = otpHash;
        user.EmailChangeOtpExpiresAt = DateTime.UtcNow.AddMinutes(OtpLifetimeMinutes);
        user.UpdatedAt = DateTime.UtcNow;

        _users.Update(user);
        await _users.SaveChangesAsync(ct);

        await _email.SendAsync(
            newEmail,
            "Confirm your new email",
            $"Your verification code is {otp}. It expires in {OtpLifetimeMinutes} minutes.",
            ct);
    }

    public async Task ConfirmEmailChangeAsync(Guid userId, ConfirmEmailChangeRequest request, CancellationToken ct = default)
    {
        var user = await _users.GetByIdAsync(userId, ct)
            ?? throw new AuthException("User not found.", 404);

        if (user.PendingEmail is null || user.EmailChangeOtpHash is null || user.EmailChangeOtpExpiresAt is null)
            throw new AuthException("No pending email change.", 400);

        if (DateTime.UtcNow > user.EmailChangeOtpExpiresAt)
            throw new AuthException("Verification code has expired.", 400);

        if (!_hasher.Verify(request.Otp, user.EmailChangeOtpHash))
            throw new AuthException("Invalid verification code.", 400);

        user.Email = user.PendingEmail;
        user.PendingEmail = null;
        user.EmailChangeOtpHash = null;
        user.EmailChangeOtpExpiresAt = null;
        user.IsEmailVerified = true;
        user.UpdatedAt = DateTime.UtcNow;

        _users.Update(user);
        await _users.SaveChangesAsync(ct);
    }

    public async Task DeleteAccountAsync(Guid userId, DeleteAccountRequest request, CancellationToken ct = default)
    {
        var user = await _users.GetByIdAsync(userId, ct)
            ?? throw new AuthException("User not found.", 404);

        if (string.IsNullOrEmpty(user.PasswordHash) || !_hasher.Verify(request.Password, user.PasswordHash))
            throw new AuthException("Password is incorrect.", 400);

        _users.Delete(user);
        await _users.SaveChangesAsync(ct);
    }

    private string GenerateOtp(out string hash)
    {
        var otp = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");
        hash = _hasher.Hash(otp);
        return otp;
    }

    private static UserResponse ToUserResponse(server.Domain.Entities.User user) => new()
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email,
        IsEmailVerified = user.IsEmailVerified
    };
}
