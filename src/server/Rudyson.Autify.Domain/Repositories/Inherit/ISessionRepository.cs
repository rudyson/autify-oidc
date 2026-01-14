using Rudyson.Autify.Domain.Entities;
using Rudyson.Autify.Domain.ValueObjects;

namespace Rudyson.Autify.Domain.Repositories.Inherit;

public interface ISessionRepository : IRepository<Session, SessionId>
{
    Task<Session?> GetByRefreshTokenAsync(RefreshTokenHash token, CancellationToken cancellationToken = default);
}
