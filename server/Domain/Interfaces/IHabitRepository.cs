using server.Domain.Entities;

namespace server.Domain.Interfaces;

public interface IHabitRepository
{
    Task<Habit?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Habit>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task AddAsync(Habit habit, CancellationToken ct = default);
    void Update(Habit habit);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
