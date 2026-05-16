using server.Domain.Entities;

namespace server.Domain.Interfaces;

public interface ITaskRepository
{
    Task<UserTask?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<UserTask>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task AddAsync(UserTask task, CancellationToken ct = default);
    void Update(UserTask task);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
