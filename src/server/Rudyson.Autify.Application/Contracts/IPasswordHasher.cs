using Rudyson.Autify.Domain.ValueObjects;

namespace Rudyson.Autify.Application.Contracts;

public interface IPasswordHasher
{
    PasswordHash Hash(string plainPassword);
    bool Verify(string plainPassword, PasswordHash hash);
}
