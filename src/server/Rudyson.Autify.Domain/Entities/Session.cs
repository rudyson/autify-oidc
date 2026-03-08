using Rudyson.Autify.Domain.Core;
using Rudyson.Autify.Domain.ValueObjects;

namespace Rudyson.Autify.Domain.Entities;

public sealed class Session : AggregateRoot<SessionId>
{
    public UserId UserId { get; private set; } = null!;
    public RefreshTokenHash RefreshTokenHash { get; private set; } = null!;
    public DateTime ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }

    public bool IsActive =>
        RevokedAt == null && ExpiresAt > DateTime.UtcNow;

    private Session() { }

    private Session(
        SessionId id,
        UserId userId,
        RefreshTokenHash refreshToken,
        DateTime expiresAt)
    {
        Id = id;
        UserId = userId;
        RefreshTokenHash = refreshToken;
        ExpiresAt = expiresAt;
        CreatedAt = DateTime.UtcNow;
    }

    public static Session Create(
        UserId userId,
        RefreshTokenHash refreshToken,
        DateTime expiresAt)
    {
        if (expiresAt <= DateTime.UtcNow)
            throw new DomainException("Session expiration must be in future");

        return new Session(
            SessionId.New(),
            userId,
            refreshToken,
            expiresAt
        );
    }

    public void Revoke()
    {
        if (RevokedAt != null)
            return;

        RevokedAt = DateTime.UtcNow;
    }

    public void RotateRefreshToken(RefreshTokenHash newToken, DateTime newExpiresAt)
    {
        if (!IsActive)
            throw new DomainException("Session is not active");

        RefreshTokenHash = newToken;
        ExpiresAt = newExpiresAt;
    }
}
