using System.ComponentModel.DataAnnotations;

namespace server.Application.DTOs.Auth;

public class ResetPasswordRequest
{
    [Required]
    public string Token { get; set; } = null!;

    [Required, MinLength(8), MaxLength(128)]
    public string NewPassword { get; set; } = null!;
}