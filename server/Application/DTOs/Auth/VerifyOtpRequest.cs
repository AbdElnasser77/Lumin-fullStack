using System.ComponentModel.DataAnnotations;

namespace server.Application.DTOs.Auth;

public class ConfirmEmailRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = null!;

    [Required, StringLength(6, MinimumLength = 6)]
    public string Otp { get; set; } = null!;
}
