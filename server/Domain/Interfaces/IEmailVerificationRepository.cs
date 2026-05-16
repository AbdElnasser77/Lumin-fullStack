using server.Domain.Entities;

namespace server.Domain.Interfaces;

public interface IEmailVerificationRepository
{
    Task<EmailVerification?> GetLatestByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task AddAsync(EmailVerification ev, CancellationToken ct = default);
    void Update(EmailVerification ev);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
