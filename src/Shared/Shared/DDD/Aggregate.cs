namespace Shared.DDD
{
    public abstract class Aggregate<TId> : Entity<TId>, IAggregate<TId>
        where TId : notnull
    {
        private readonly List<IDomainEvent> _domainEvents = [];

        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public IDomainEvent[] ClearDomainEvents()
        {
            var events = _domainEvents.ToArray();
            _domainEvents.Clear();

            return events;
        }
        
        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
        
        public void RemoveDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }
    }
}
