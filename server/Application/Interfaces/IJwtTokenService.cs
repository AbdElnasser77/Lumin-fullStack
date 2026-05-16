using server.Domain.Entities;

namespace server.Application.Interfaces;

public interface IJwtTokenService
{
    (string token, DateTime expiresAt) GenerateToken(User user);
}
