using Microsoft.EntityFrameworkCore;
using server.Domain.Entities;
using server.Domain.Interfaces;
using server.Infrastructure.Persistence;

namespace server.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _db;

    public TaskRepository(AppDbContext db) => _db = db;

    public Task<UserTask?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _db.Tasks.FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task<IReadOnlyList<UserTask>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await _db.Tasks.Where(t => t.UserId == userId).ToListAsync(ct);

    public async Task AddAsync(UserTask task, CancellationToken ct = default)
        => await _db.Tasks.AddAsync(task, ct);

    public void Update(UserTask task)
        => _db.Tasks.Update(task);

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}
