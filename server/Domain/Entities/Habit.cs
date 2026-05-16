using server.Domain.Common;
using server.Domain.Enums;

namespace server.Domain.Entities;

public class Habit : BaseEntity
{
    public string Name { get; set; } = null!;
    public HabitFrequency Frequency { get; set; }
    public HabitStatus Status { get; set; } = HabitStatus.Active;
    public int Streak { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
