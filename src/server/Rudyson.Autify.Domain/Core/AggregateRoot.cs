namespace Rudyson.Autify.Domain.Core;

public abstract class AggregateRoot<TId> : IAggregateRoot
{
    public TId Id { get; protected set; } = default!;

    private readonly List<object> _domainEvents = new();
    public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(object eventItem) => _domainEvents.Add(eventItem);
    public void ClearDomainEvents() => _domainEvents.Clear();
}
