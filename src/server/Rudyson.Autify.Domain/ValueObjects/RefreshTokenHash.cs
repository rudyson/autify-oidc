using Rudyson.Autify.Domain.Core;

namespace Rudyson.Autify.Domain.ValueObjects;

public sealed class RefreshTokenHash : ValueObject
{
    public string Value { get; init; }

    public RefreshTokenHash(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new DomainException("Refresh token hash is empty");

        Value = hash;
    }

    public static RefreshTokenHash FromHash(string hash) => new(hash);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
