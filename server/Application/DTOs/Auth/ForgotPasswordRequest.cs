using System.ComponentModel.DataAnnotations;

namespace server.Application.DTOs.Auth;

public class ForgotPasswordRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = null!;

    [Required, Url]
    public string RedirectUrl { get; set; } = null!;
}