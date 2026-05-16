using server.Domain.Common;
using server.Domain.Enums;

namespace server.Domain.Entities;

public class UserTask : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public UserTaskStatus Status { get; set; } = UserTaskStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
