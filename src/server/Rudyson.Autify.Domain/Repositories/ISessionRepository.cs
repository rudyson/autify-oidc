using Rudyson.Autify.Domain.Entities;
using Rudyson.Autify.Domain.ValueObjects;

namespace Rudyson.Autify.Domain.Repositories;

public interface ISessionRepository
{
    Task<Session?> GetByRefreshTokenAsync(
        RefreshToken token,
        CancellationToken ct);

    Task AddAsync(Session session, CancellationToken ct);
    Task UpdateAsync(Session session, CancellationToken ct);
}
