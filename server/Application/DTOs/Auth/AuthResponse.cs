namespace server.Application.DTOs.Auth;

public class AuthResponse
{
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public string RefreshToken { get; set; } = null!;
    public DateTime RefreshTokenExpiresAt { get; set; }
    public UserResponse User { get; set; } = null!;
}
