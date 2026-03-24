using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Events;

namespace mtkpm.Infrastructure.Services.Notifications
{
    /// <summary>
    /// SMS Notification Observer - G?i SMS thông báo
    /// Observer Pattern - Another concrete observer
    /// </summary>
    public class SMSNotificationService : INotificationObserver
    {
        private readonly ILoggerService _logger;

        public string ObserverName => "SMSNotification";

        public SMSNotificationService(ILoggerService logger)
        {
            _logger = logger;
        }

        public async Task OnOrderCreatedAsync(OrderCreatedEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"?? Sending order creation SMS to user {@event.UserId}. Order: {@event.OrderNumber}", "SMSNotification");

            await Task.Delay(100, cancellationToken);

            _logger.LogInfo($"? Order creation SMS sent", "SMSNotification");
        }

        public async Task OnOrderConfirmedAsync(OrderConfirmedEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"?? Sending order confirmation SMS for {@event.OrderNumber}", "SMSNotification");

            await Task.Delay(100, cancellationToken);

            _logger.LogInfo($"? Order confirmation SMS sent", "SMSNotification");
        }

        public async Task OnOrderShippedAsync(OrderShippedEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"?? Sending shipment tracking SMS with tracking number {@event.TrackingNumber}", "SMSNotification");

            await Task.Delay(100, cancellationToken);

            _logger.LogInfo($"? Shipment tracking SMS sent", "SMSNotification");
        }

        public async Task OnOrderDeliveredAsync(OrderDeliveredEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"?? Sending delivery confirmation SMS for {@event.OrderNumber}", "SMSNotification");

            await Task.Delay(100, cancellationToken);

            _logger.LogInfo($"? Delivery confirmation SMS sent", "SMSNotification");
        }

        public async Task OnOrderCancelledAsync(OrderCancelledEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"?? Sending order cancellation SMS for {@event.OrderNumber}", "SMSNotification");

            await Task.Delay(100, cancellationToken);

            _logger.LogInfo($"? Order cancellation SMS sent", "SMSNotification");
        }

        public async Task OnPaymentCompletedAsync(PaymentCompletedEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"?? Sending payment confirmation SMS. Amount: {@event.Amount:C}", "SMSNotification");

            await Task.Delay(100, cancellationToken);

            _logger.LogInfo($"? Payment confirmation SMS sent", "SMSNotification");
        }

        public async Task OnPaymentFailedAsync(PaymentFailedEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"?? Sending payment failure SMS. Amount: {@event.Amount:C}", "SMSNotification");

            await Task.Delay(100, cancellationToken);

            _logger.LogInfo($"? Payment failure SMS sent", "SMSNotification");
        }

        public async Task OnPaymentRefundedAsync(PaymentRefundedEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"?? Sending refund SMS. Amount: {@event.RefundAmount:C}", "SMSNotification");

            await Task.Delay(100, cancellationToken);

            _logger.LogInfo($"? Refund SMS sent", "SMSNotification");
        }
    }
}
