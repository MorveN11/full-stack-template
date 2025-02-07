namespace SharedKernel.Domain;

public abstract class Entity : Register
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public Guid Id { get; init; } = Guid.CreateVersion7();

    public List<IDomainEvent> DomainEvents => [.. _domainEvents];

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void Raise(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
