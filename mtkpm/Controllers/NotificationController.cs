using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Application.Common.DTOs.Common;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Events;

namespace mtkpm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize(Roles = "Admin")]
    public class NotificationController : ControllerBase
    {
        private readonly IEventPublisher _eventPublisher;

        public NotificationController(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// L?y danh sách observers ?ang ??ng ký
        /// Observer Pattern - Hi?n th? t?t c? subscribers
        /// </summary>
        [HttpGet("subscribers")]
        [ProducesResponseType(typeof(SubscribersResponse), StatusCodes.Status200OK)]
        public IActionResult GetSubscribers()
        {
            var count = _eventPublisher.GetSubscriberCount();
            var names = _eventPublisher.GetSubscriberNames();

            var response = new SubscribersResponse
            {
                TotalSubscribers = count,
                Subscribers = names,
                Message = $"{count} notification observers are listening for events"
            };

            return Ok(ApiResponse<SubscribersResponse>.SuccessResponse(response));
        }

        /// <summary>
        /// G?i test notification cho order created event
        /// Observer Pattern - Demo: t?t c? observers s? nh?n event
        /// </summary>
        [HttpPost("test/order-created")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> TestOrderCreatedEvent()
        {
            var @event = new OrderCreatedEvent(
                orderId: 1,
                userId: 1,
                orderNumber: "ORD-TEST-001",
                totalAmount: 1000000,
                shippingAddress: "123 Test Street, Ho Chi Minh City"
            );

            await _eventPublisher.PublishAsync(@event);

            return Ok(ApiResponse<object>.SuccessResponse(
                new { message = "Order created event published to all observers" }
            ));
        }

        /// <summary>
        /// G?i test notification cho payment completed event
        /// </summary>
        [HttpPost("test/payment-completed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> TestPaymentCompletedEvent()
        {
            var @event = new PaymentCompletedEvent(
                orderId: 1,
                userId: 1,
                transactionId: "TXN-TEST-001",
                amount: 1000000,
                paymentMethod: "CreditCard"
            );

            await _eventPublisher.PublishAsync(@event);

            return Ok(ApiResponse<object>.SuccessResponse(
                new { message = "Payment completed event published to all observers" }
            ));
        }

        /// <summary>
        /// G?i test notification cho order shipped event
        /// </summary>
        [HttpPost("test/order-shipped")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> TestOrderShippedEvent()
        {
            var @event = new OrderShippedEvent(
                orderId: 1,
                userId: 1,
                orderNumber: "ORD-TEST-001",
                trackingNumber: "TRACK-001-ABC"
            );

            await _eventPublisher.PublishAsync(@event);

            return Ok(ApiResponse<object>.SuccessResponse(
                new { message = "Order shipped event published to all observers" }
            ));
        }

        /// <summary>
        /// G?i test notification cho payment failed event
        /// </summary>
        [HttpPost("test/payment-failed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> TestPaymentFailedEvent()
        {
            var @event = new PaymentFailedEvent(
                orderId: 1,
                userId: 1,
                amount: 1000000,
                reason: "Card declined by bank"
            );

            await _eventPublisher.PublishAsync(@event);

            return Ok(ApiResponse<object>.SuccessResponse(
                new { message = "Payment failed event published to all observers" }
            ));
        }

        /// <summary>
        /// G?i test notification cho order cancelled event
        /// </summary>
        [HttpPost("test/order-cancelled")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> TestOrderCancelledEvent()
        {
            var @event = new OrderCancelledEvent(
                orderId: 1,
                userId: 1,
                orderNumber: "ORD-TEST-001",
                reason: "Customer requested cancellation"
            );

            await _eventPublisher.PublishAsync(@event);

            return Ok(ApiResponse<object>.SuccessResponse(
                new { message = "Order cancelled event published to all observers" }
            ));
        }

        /// <summary>
        /// H??ng d?n Observer Pattern
        /// </summary>
        [HttpGet("guide")]
        [AllowAnonymous]
        public IActionResult GetObserverPatternGuide()
        {
            var guide = @"
Observer Pattern - Notification System

**M?c Tiêu:**
Thông báo ??n multiple observers khi có events x?y ra, không c?n coupling.

**C?u Trúc:**
- Subject (EventPublisher): Qu?n lý observers, phát events
- Observer (INotificationObserver): Interface mà observers implement
- ConcreteObservers: EmailNotificationService, SMSNotificationService, PushNotificationService

**Events:**
- OrderCreatedEvent: ??n hàng ???c t?o
- OrderConfirmedEvent: ??n hàng ???c xác nh?n
- OrderShippedEvent: ??n hàng ???c g?i ?i
- OrderDeliveredEvent: ??n hàng ???c giao
- OrderCancelledEvent: ??n hàng b? h?y
- PaymentCompletedEvent: Thanh toán thành công
- PaymentFailedEvent: Thanh toán th?t b?i
- PaymentRefundedEvent: Hoàn ti?n

**Ví D? S? D?ng:**

1. L?y danh sách observers:
   GET /api/notification/subscribers

2. Test order created event:
   POST /api/notification/test/order-created

3. Test payment completed:
   POST /api/notification/test/payment-completed

**L?i Ích Observer Pattern:**
? Loose Coupling - Subject không bi?t chi ti?t observers
? Dynamic Subscription - Thêm/xóa observers lúc runtime
? Broadcast Communication - M?t event g?i t?i nhi?u observers
? Separation of Concerns - M?i observer có trách nhi?m riêng
? Easy to Extend - Thêm observer m?i mà không s?a code c?

**Real-World Usage:**
- Khi order ???c t?o, t?t c? observers (Email, SMS, Push) ??ng th?i nh?n notification
- Khi thanh toán th?t b?i, customer ???c thông báo qua email + SMS + push
- D? m? r?ng: thêm Slack notification, Discord notification, etc.
";

            return Ok(new { guide });
        }
    }

    public class SubscribersResponse
    {
        public int TotalSubscribers { get; set; }
        public List<string> Subscribers { get; set; }
        public string Message { get; set; }
    }
}
