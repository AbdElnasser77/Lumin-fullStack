using server.Domain.Common;

namespace server.Domain.Entities;

public class EmailVerification : BaseEntity
{
    public Guid UserId { get; set; }
    public string OtpHash { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
}
