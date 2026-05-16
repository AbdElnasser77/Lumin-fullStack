using System.ComponentModel.DataAnnotations;

namespace server.Application.DTOs.Profile;

public class UpdateProfileRequest
{
    [Required, MaxLength(100)] public string FirstName { get; set; } = null!;
    [Required, MaxLength(100)] public string LastName { get; set; } = null!;
}
