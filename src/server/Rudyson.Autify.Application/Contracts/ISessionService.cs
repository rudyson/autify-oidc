namespace Rudyson.Autify.Application.Contracts;

public interface ISessionService
{
    Task RevokeSessionAsync(Guid sessionId, CancellationToken cancellationToken = default);
    Task RevokeSessionAsync(string refreshToken, CancellationToken cancellationToken = default);
}
