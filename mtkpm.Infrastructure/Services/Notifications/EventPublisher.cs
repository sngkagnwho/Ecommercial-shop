using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Events;

namespace mtkpm.Infrastructure.Services.Notifications
{
    /// <summary>
    /// Event Publisher (Subject) - Qu?n lý observers và phát events
    /// Observer Pattern - Subject implementation
    /// </summary>
    public class EventPublisher : IEventPublisher
    {
        private readonly List<INotificationObserver> _observers = new();
        private readonly ILoggerService _logger;

        public EventPublisher(ILoggerService logger)
        {
            _logger = logger;
        }

        public void Subscribe(INotificationObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
                _logger.LogInfo($"? Observer '{observer.ObserverName}' subscribed. Total subscribers: {_observers.Count}", "EventPublisher");
            }
        }

        public void Unsubscribe(INotificationObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            if (_observers.Remove(observer))
            {
                _logger.LogInfo($"? Observer '{observer.ObserverName}' unsubscribed. Total subscribers: {_observers.Count}", "EventPublisher");
            }
        }

        public async Task PublishAsync(DomainEvent @event, CancellationToken cancellationToken = default)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            _logger.LogInfo($"?? Publishing event: {@event.GetType().Name}. Subscribers: {_observers.Count}", "EventPublisher");

            var tasks = new List<Task>();

            foreach (var observer in _observers)
            {
                try
                {
                    var task = @event switch
                    {
                        OrderCreatedEvent orderCreated => observer.OnOrderCreatedAsync(orderCreated, cancellationToken),
                        OrderConfirmedEvent orderConfirmed => observer.OnOrderConfirmedAsync(orderConfirmed, cancellationToken),
                        OrderShippedEvent orderShipped => observer.OnOrderShippedAsync(orderShipped, cancellationToken),
                        OrderDeliveredEvent orderDelivered => observer.OnOrderDeliveredAsync(orderDelivered, cancellationToken),
                        OrderCancelledEvent orderCancelled => observer.OnOrderCancelledAsync(orderCancelled, cancellationToken),
                        PaymentCompletedEvent paymentCompleted => observer.OnPaymentCompletedAsync(paymentCompleted, cancellationToken),
                        PaymentFailedEvent paymentFailed => observer.OnPaymentFailedAsync(paymentFailed, cancellationToken),
                        PaymentRefundedEvent paymentRefunded => observer.OnPaymentRefundedAsync(paymentRefunded, cancellationToken),
                        _ => Task.CompletedTask
                    };

                    tasks.Add(task);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error notifying observer '{observer.ObserverName}': {ex.Message}", "EventPublisher");
                }
            }

            // Notify all observers in parallel
            await Task.WhenAll(tasks);

            _logger.LogInfo($"? Event {@event.GetType().Name} published to {_observers.Count} observers", "EventPublisher");
        }

        public int GetSubscriberCount()
        {
            return _observers.Count;
        }

        public List<string> GetSubscriberNames()
        {
            return _observers.Select(o => o.ObserverName).ToList();
        }
    }
}
