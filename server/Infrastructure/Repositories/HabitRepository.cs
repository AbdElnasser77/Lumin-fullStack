using Microsoft.EntityFrameworkCore;
using server.Domain.Entities;
using server.Domain.Interfaces;
using server.Infrastructure.Persistence;

namespace server.Infrastructure.Repositories;

public class HabitRepository : IHabitRepository
{
    private readonly AppDbContext _db;

    public HabitRepository(AppDbContext db) => _db = db;

    public Task<Habit?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _db.Habits.FirstOrDefaultAsync(h => h.Id == id, ct);

    public async Task<IReadOnlyList<Habit>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await _db.Habits.Where(h => h.UserId == userId).ToListAsync(ct);

    public async Task AddAsync(Habit habit, CancellationToken ct = default)
        => await _db.Habits.AddAsync(habit, ct);

    public void Update(Habit habit)
        => _db.Habits.Update(habit);

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}
