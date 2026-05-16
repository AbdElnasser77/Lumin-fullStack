using System.ComponentModel.DataAnnotations;

namespace server.Application.DTOs.Profile;

public class RequestEmailChangeRequest
{
    [Required, EmailAddress, MaxLength(256)] public string NewEmail { get; set; } = null!;
}
