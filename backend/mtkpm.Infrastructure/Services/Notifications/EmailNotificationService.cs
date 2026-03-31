using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Events;

namespace mtkpm.Infrastructure.Services.Notifications
{
    /// <summary>
    /// Email Notification Observer - G?i email th�ng b�o
    /// Observer Pattern - Observer concrete implementation
    /// </summary>
    public class EmailNotificationService : INotificationObserver
    {
        private readonly ILoggerService _logger;

        public string ObserverName => "EmailNotification";

        public EmailNotificationService(ILoggerService logger)
        {
            _logger = logger;
        }

        public async Task OnOrderCreatedAsync(OrderCreatedEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"✔ Sending order creation email to user {@event.UserId} for order {@event.OrderNumber}", "EmailNotification");

            // Simulate sending email
            await Task.Delay(200, cancellationToken);

            _logger.LogInfo($"✓ Order creation email sent for {@event.OrderNumber}", "EmailNotification");
        }

        public async Task OnOrderConfirmedAsync(OrderConfirmedEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"✔ Sending order confirmation email for {@event.OrderNumber}", "EmailNotification");

            await Task.Delay(200, cancellationToken);

            _logger.LogInfo($"✓ Order confirmation email sent for {@event.OrderNumber}", "EmailNotification");
        }

        public async Task OnOrderShippedAsync(OrderShippedEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"✔ Sending shipment notification email with tracking {@event.TrackingNumber}", "EmailNotification");

            await Task.Delay(200, cancellationToken);

            _logger.LogInfo($"✓ Shipment notification sent with tracking {@event.TrackingNumber}", "EmailNotification");
        }

        public async Task OnOrderDeliveredAsync(OrderDeliveredEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"✔ Sending delivery confirmation email for {@event.OrderNumber}", "EmailNotification");

            await Task.Delay(200, cancellationToken);

            _logger.LogInfo($"✓ Delivery confirmation email sent for {@event.OrderNumber}", "EmailNotification");
        }

        public async Task OnOrderCancelledAsync(OrderCancelledEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"✔ Sending order cancellation email for {@event.OrderNumber}. Reason: {@event.Reason}", "EmailNotification");

            await Task.Delay(200, cancellationToken);

            _logger.LogInfo($"✓ Order cancellation email sent for {@event.OrderNumber}", "EmailNotification");
        }

        public async Task OnPaymentCompletedAsync(PaymentCompletedEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"✔ Sending payment confirmation email for order {@event.OrderId}. Amount: {@event.Amount:C}, Method: {@event.PaymentMethod}", "EmailNotification");

            await Task.Delay(200, cancellationToken);

            _logger.LogInfo($"✓ Payment confirmation email sent", "EmailNotification");
        }

        public async Task OnPaymentFailedAsync(PaymentFailedEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"✔ Sending payment failure email for order {@event.OrderId}. Reason: {@event.Reason}", "EmailNotification");

            await Task.Delay(200, cancellationToken);

            _logger.LogInfo($"✓ Payment failure email sent", "EmailNotification");
        }

        public async Task OnPaymentRefundedAsync(PaymentRefundedEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"✔ Sending refund notification email. Amount: {@event.RefundAmount:C}", "EmailNotification");

            await Task.Delay(200, cancellationToken);

            _logger.LogInfo($"✓ Refund notification email sent", "EmailNotification");
        }
    }
}

