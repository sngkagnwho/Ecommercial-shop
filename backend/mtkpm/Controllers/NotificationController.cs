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
        /// Lấy danh sách các observer đang đăng ký
        /// Observer Pattern - Hiện thị tất cả subscribers
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
                Message = $"{count} observer đang lắng nghe các sự kiện"
            };

            return Ok(ApiResponse<SubscribersResponse>.SuccessResponse(response));
        }

        /// <summary>
        /// Gửi test notification cho sự kiện tạo đơn hàng
        /// Observer Pattern - Demo: tất cả observers sẽ nhận event
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
                new { message = "Sự kiện tạo đơn hàng đã được công bố tới tất cả observers" }
            ));
        }

        /// <summary>
        /// Gửi test notification cho sự kiện thanh toán hoàn thành
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
                new { message = "Sự kiện thanh toán hoàn thành đã được công bố tới tất cả observers" }
            ));
        }

        /// <summary>
        /// Gửi test notification cho sự kiện đơn hàng được gửi đi
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
                new { message = "Sự kiện đơn hàng được gửi đi đã được công bố tới tất cả observers" }
            ));
        }

        /// <summary>
        /// Gửi test notification cho sự kiện thanh toán thất bại
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
                new { message = "Sự kiện thanh toán thất bại đã được công bố tới tất cả observers" }
            ));
        }

        /// <summary>
        /// Gửi test notification cho sự kiện đơn hàng bị hủy
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
                new { message = "Sự kiện đơn hàng bị hủy đã được công bố tới tất cả observers" }
            ));
        }

        /// <summary>
        /// Hướng dẫn sử dụng Observer Pattern
        /// </summary>
        [HttpGet("guide")]
        [AllowAnonymous]
        public IActionResult GetObserverPatternGuide()
        {
            var guide = @"
Observer Pattern - Hệ thống Thông báo

**Mục đích:**
Thông báo tới nhiều observers khi có sự kiện xảy ra mà không cần coupling.

**Cấu Trúc:**
- Subject (EventPublisher): Quản lý observers, phát events
- Observer (INotificationObserver): Interface mà observers implement
- ConcreteObservers: EmailNotificationService, SMSNotificationService, PushNotificationService

**Các Sự Kiện:**
- OrderCreatedEvent: Đơn hàng được tạo
- OrderConfirmedEvent: Đơn hàng được xác nhận
- OrderShippedEvent: Đơn hàng được gửi đi
- OrderDeliveredEvent: Đơn hàng được giao
- OrderCancelledEvent: Đơn hàng bị hủy
- PaymentCompletedEvent: Thanh toán hoàn thành
- PaymentFailedEvent: Thanh toán thất bại
- PaymentRefundedEvent: Hoàn tiền

**Ví Dụ Sử Dụng:**

1. Lấy danh sách observers:
   GET /api/notification/subscribers

2. Test sự kiện tạo đơn hàng:
   POST /api/notification/test/order-created

3. Test thanh toán hoàn thành:
   POST /api/notification/test/payment-completed

**Lợi Ích Observer Pattern:**
- Loose Coupling: Subject không biết chi tiết observers
- Dynamic Subscription: Thêm/xóa observers lúc runtime
- Broadcast Communication: Một sự kiện gửi tới nhiều observers
- Separation of Concerns: Mỗi observer có trách nhiệm riêng
- Easy to Extend: Thêm observer mới mà không sửa code cũ

**Sử Dụng Thực Tế:**
- Khi đơn hàng được tạo, tất cả observers (Email, SMS, Push) cùng lúc nhận thông báo
- Khi thanh toán thất bại, khách hàng được thông báo qua email + SMS + push
- Dễ mở rộng: thêm Slack notification, Discord notification, v.v.
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
