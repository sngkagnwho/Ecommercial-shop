namespace mtkpm.Domain.Events
{
    /// <summary>
    /// Base class cho tất cả domain events
    /// Observer Pattern - Event để notify subscribers
    /// </summary>
    public abstract class DomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public Guid AggregateId { get; protected set; }
    }
}
