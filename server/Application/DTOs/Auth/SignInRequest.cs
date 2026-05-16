using System.ComponentModel.DataAnnotations;

namespace server.Application.DTOs.Auth;

public class SignInRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}
