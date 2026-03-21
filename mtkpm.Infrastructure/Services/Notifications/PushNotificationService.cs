using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Events;

namespace mtkpm.Infrastructure.Services.Notifications
{
    /// <summary>
    /// Push Notification Observer - G?i push notification
    /// Observer Pattern - Third concrete observer
    /// </summary>
    public class PushNotificationService : INotificationObserver
    {
        private readonly ILoggerService _logger;

        public string ObserverName => "PushNotification";

        public PushNotificationService(ILoggerService logger)
        {
            _logger = logger;
        }

        public async Task OnOrderCreatedAsync(OrderCreatedEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"?? Sending push notification: Order {@event.OrderNumber} created. Total: {@event.TotalAmount:C}", "PushNotification");

            await Task.Delay(50, cancellationToken);

            _logger.LogInfo($"? Order creation push notification sent", "PushNotification");
        }

        public async Task OnOrderConfirmedAsync(OrderConfirmedEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"?? Sending push notification: Order {@event.OrderNumber} confirmed", "PushNotification");

            await Task.Delay(50, cancellationToken);

            _logger.LogInfo($"? Order confirmation push notification sent", "PushNotification");
        }

        public async Task OnOrderShippedAsync(OrderShippedEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"?? Sending push notification: Order {@event.OrderNumber} shipped with tracking {@event.TrackingNumber}", "PushNotification");

            await Task.Delay(50, cancellationToken);

            _logger.LogInfo($"? Order shipment push notification sent", "PushNotification");
        }

        public async Task OnOrderDeliveredAsync(OrderDeliveredEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"?? Sending push notification: Order {@event.OrderNumber} delivered", "PushNotification");

            await Task.Delay(50, cancellationToken);

            _logger.LogInfo($"? Order delivery push notification sent", "PushNotification");
        }

        public async Task OnOrderCancelledAsync(OrderCancelledEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"?? Sending push notification: Order {@event.OrderNumber} cancelled", "PushNotification");

            await Task.Delay(50, cancellationToken);

            _logger.LogInfo($"? Order cancellation push notification sent", "PushNotification");
        }

        public async Task OnPaymentCompletedAsync(PaymentCompletedEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"?? Sending push notification: Payment completed via {@event.PaymentMethod}. Amount: {@event.Amount:C}", "PushNotification");

            await Task.Delay(50, cancellationToken);

            _logger.LogInfo($"? Payment completion push notification sent", "PushNotification");
        }

        public async Task OnPaymentFailedAsync(PaymentFailedEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"?? Sending push notification: Payment failed. Reason: {@event.Reason}", "PushNotification");

            await Task.Delay(50, cancellationToken);

            _logger.LogInfo($"? Payment failure push notification sent", "PushNotification");
        }

        public async Task OnPaymentRefundedAsync(PaymentRefundedEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"?? Sending push notification: Refund processed. Amount: {@event.RefundAmount:C}", "PushNotification");

            await Task.Delay(50, cancellationToken);

            _logger.LogInfo($"? Refund push notification sent", "PushNotification");
        }
    }
}
