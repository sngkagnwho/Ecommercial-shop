namespace mtkpm.Domain.Events
{
    /// <summary>
    /// Base class cho t?t c? domain events
    /// Observer Pattern - Event ?? notify subscribers
    /// </summary>
    public abstract class DomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public Guid AggregateId { get; protected set; }
    }
}
