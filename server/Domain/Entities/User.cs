using server.Domain.Common;

namespace server.Domain.Entities;

public class User : BaseEntity
{
    public ICollection<UserTask> Tasks { get; set; } = new List<UserTask>();
    public ICollection<Habit> Habits { get; set; } = new List<Habit>();
    public ICollection<EmailVerification> EmailVerifications { get; set; } = new List<EmailVerification>();

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;

    public string? PasswordHash { get; set; }
    public bool CheckTerms { get; set; }
    public bool IsEmailVerified { get; set; }

    public string? OtpHash { get; set; }
    public DateTime? OtpExpiresAt { get; set; }
    public DateTime? OtpVerifiedAt { get; set; }

    public string? RefreshTokenHash { get; set; }
    public DateTime? RefreshTokenExpiresAt { get; set; }

    public string? PendingEmail { get; set; }
    public string? EmailChangeOtpHash { get; set; }
    public DateTime? EmailChangeOtpExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
