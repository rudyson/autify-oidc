using Microsoft.EntityFrameworkCore;
using Rudyson.Autify.Domain.Entities;
using Rudyson.Autify.Domain.Repositories.Inherit;
using Rudyson.Autify.Domain.ValueObjects;

namespace Rudyson.Autify.Infrastructure.Persistence;

public class SessionRepository : EfRepository<Session, SessionId>, ISessionRepository
{
    public SessionRepository(AutifyContext context) : base(context) { }

    public async Task<Session?> GetByRefreshTokenAsync(RefreshTokenHash token, CancellationToken ct) =>
        await _context.Sessions.FirstOrDefaultAsync(s => s.RefreshTokenHash == token, ct);
}
