namespace server.Application.DTOs.Auth;

public class ForgotPasswordResponse
{
    public string Message { get; set; } = null!;
    public string? ResetToken { get; set; }
}