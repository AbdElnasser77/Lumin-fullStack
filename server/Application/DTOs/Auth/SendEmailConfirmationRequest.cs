using System.ComponentModel.DataAnnotations;

namespace server.Application.DTOs.Auth;

public class SendEmailConfirmationRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = null!;
}
