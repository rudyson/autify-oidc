using Rudyson.Autify.Domain.Core;

namespace Rudyson.Autify.Domain.ValueObjects;

public sealed class UserId : ValueObject
{
    public Guid Value { get; init; }

    public UserId(Guid value)
    {
        Value = value;
    }

    public static UserId New()
    {
        return new UserId(Guid.NewGuid());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
