namespace Payment.API.Domain.SeedWork;

// Payment.API/Domain/SeedWork/AggregateRoot.cs
public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public Guid Id { get; protected set; }
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    // Padrão: Versionamento para concorrência
    public int Version { get; protected set; } = -1;

    protected void ApplyChange(IDomainEvent @event, bool isNew = true)
    {
        // Pattern: Event Sourcing (opcional)
        When(@event);
        if (isNew)
        {
            AddDomainEvent(@event);
        }
    }

    protected abstract void When(IDomainEvent @event);
}
