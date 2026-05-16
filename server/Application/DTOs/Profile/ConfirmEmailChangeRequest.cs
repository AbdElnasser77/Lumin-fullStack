using System.ComponentModel.DataAnnotations;

namespace server.Application.DTOs.Profile;

public class ConfirmEmailChangeRequest
{
    [Required, StringLength(6, MinimumLength = 6)] public string Otp { get; set; } = null!;
}
