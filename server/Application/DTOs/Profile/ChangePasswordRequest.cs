using System.ComponentModel.DataAnnotations;

namespace server.Application.DTOs.Profile;

public class ChangePasswordRequest
{
    [Required] public string CurrentPassword { get; set; } = null!;
    [Required, MinLength(8), MaxLength(128)] public string NewPassword { get; set; } = null!;
}
