using Rudyson.Autify.Domain.Core;

namespace Rudyson.Autify.Domain.ValueObjects;

public sealed class PasswordHash : ValueObject
{
    public string Value { get; }

    private PasswordHash(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Password hash is empty");

        Value = value;
    }

    public static PasswordHash FromHash(string hash) => new(hash);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
