using server.Application.DTOs.Auth;

namespace server.Application.Interfaces;

public interface IAuthService
{
    Task<UserResponse> SignUpAsync(SignUpRequest request, CancellationToken ct = default);
    Task<AuthResponse> SignInAsync(SignInRequest request, CancellationToken ct = default);
    Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken ct = default);
    Task SendEmailConfirmationAsync(SendEmailConfirmationRequest request, CancellationToken ct = default);
    Task<AuthResponse> ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken ct = default);
    Task ResetPasswordAsync(ResetPasswordRequest request, CancellationToken ct = default);
    Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct = default);
}
