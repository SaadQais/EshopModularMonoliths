using MediatR;

namespace Shared.DDD
{
    public interface IDomainEvent : INotification
    {
        Guid Id => Guid.NewGuid();
        DateTime OccurredOn => DateTime.UtcNow;
        string EventType => GetType().AssemblyQualifiedName ?? GetType().Name;
    }
}
