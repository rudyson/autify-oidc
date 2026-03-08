using Rudyson.Autify.Application.Contracts;
using Rudyson.Autify.Domain.ValueObjects;

namespace Rudyson.Autify.Infrastructure.Services;

public class BCryptPasswordHasher : IPasswordHasher
{
    public PasswordHash Hash(string plainPassword)
    {
        var hashed = BCrypt.Net.BCrypt.HashPassword(plainPassword);
        return PasswordHash.FromHash(hashed);
    }
    public bool Verify(string plainPassword, PasswordHash hash)
    {
        return BCrypt.Net.BCrypt.Verify(plainPassword, hash.Value);
    }
}
