using Rudyson.Autify.Domain.Core;

namespace Rudyson.Autify.Domain.ValueObjects;

public sealed class SessionId : ValueObject
{
    public Guid Value { get; }

    private SessionId(Guid value) => Value = value;

    public static SessionId New() => new(Guid.NewGuid());

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
