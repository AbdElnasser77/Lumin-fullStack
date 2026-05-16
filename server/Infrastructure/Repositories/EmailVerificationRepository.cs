using Microsoft.EntityFrameworkCore;
using server.Domain.Entities;
using server.Domain.Interfaces;
using server.Infrastructure.Persistence;

namespace server.Infrastructure.Repositories;

public class EmailVerificationRepository : IEmailVerificationRepository
{
    private readonly AppDbContext _db;

    public EmailVerificationRepository(AppDbContext db) => _db = db;

    public Task<EmailVerification?> GetLatestByUserIdAsync(Guid userId, CancellationToken ct = default)
        => _db.EmailVerifications
            .Where(ev => ev.UserId == userId)
            .OrderByDescending(ev => ev.CreatedAt)
            .FirstOrDefaultAsync(ct);

    public async Task AddAsync(EmailVerification ev, CancellationToken ct = default)
        => await _db.EmailVerifications.AddAsync(ev, ct);

    public void Update(EmailVerification ev)
        => _db.EmailVerifications.Update(ev);

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}
