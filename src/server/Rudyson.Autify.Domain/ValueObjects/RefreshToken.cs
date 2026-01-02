using Rudyson.Autify.Domain.Core;

namespace Rudyson.Autify.Domain.ValueObjects;

public sealed class RefreshToken : ValueObject
{
    public string Hash { get; }

    private RefreshToken(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new DomainException("Refresh token hash is empty");

        Hash = hash;
    }

    public static RefreshToken FromHash(string hash) => new(hash);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Hash;
    }
}
