using System.ComponentModel.DataAnnotations;

namespace server.Application.DTOs.Profile;

public class DeleteAccountRequest
{
    [Required] public string Password { get; set; } = null!;
}
