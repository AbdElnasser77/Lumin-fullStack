using System.ComponentModel.DataAnnotations;

namespace server.Application.DTOs.Auth;

public class SignUpRequest
{
    [Required, MaxLength(100)]
    public string FirstName { get; set; } = null!;

    [Required, MaxLength(100)]
    public string LastName { get; set; } = null!;

    [Required, EmailAddress, MaxLength(256)]
    public string Email { get; set; } = null!;

    [Required, MinLength(8), MaxLength(128)]
    public string Password { get; set; } = null!;

    [Required]
    public bool CheckTerms { get; set; }
}
