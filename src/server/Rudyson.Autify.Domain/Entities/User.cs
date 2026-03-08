using Rudyson.Autify.Domain.Core;
using Rudyson.Autify.Domain.Enums;
using Rudyson.Autify.Domain.ValueObjects;

namespace Rudyson.Autify.Domain.Entities;

public sealed class User : AggregateRoot<UserId>
{
    public Email Email { get; private set; } = null!;
    public PasswordHash PasswordHash { get; private set; } = null!;
    public UserStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private User() { }
    private User(UserId id, Email email, PasswordHash passwordHash)
    {
        Id = id;
        Email = email;
        PasswordHash = passwordHash;
        Status = UserStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }

    public static User Register(Email email, PasswordHash passwordHash)
    {
        return new User(
            UserId.New(),
            email,
            passwordHash
        );
    }

    public void Suspend()
    {
        if (Status == UserStatus.Deleted)
            throw new DomainException("User already deleted");

        Status = UserStatus.Suspended;
    }

    public void Activate()
    {
        if (Status == UserStatus.Deleted)
            throw new DomainException("Deleted user cannot be activated");

        Status = UserStatus.Active;
    }

    public Session CreateSession(
        RefreshTokenHash refreshToken,
        DateTime expiresAt)
    {
        if (Status != UserStatus.Active)
            throw new DomainException("User is not active");

        return Session.Create(
            userId: Id,
            refreshToken: refreshToken,
            expiresAt: expiresAt
        );
    }


    public void ChangePassword(PasswordHash newHash)
    {
        PasswordHash = newHash;
    }
}
