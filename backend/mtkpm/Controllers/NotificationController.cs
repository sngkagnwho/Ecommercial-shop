using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Application.Common.DTOs.Common;
using mtkpm.Application.Common.DTOs.Notification;
using mtkpm.Application.Features.NotificationMethods.Commands.SubscribeNotificationMethod;
using mtkpm.Application.Features.NotificationMethods.Commands.UnsubscribeNotificationMethod;
using mtkpm.Application.Features.NotificationMethods.Queries.GetNotificationMethods;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Events;
using MediatR;

namespace mtkpm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize(Roles = "Admin")]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IEventPublisher _eventPublisher;

        public NotificationController(
            IMediator mediator,
            IEventPublisher eventPublisher)
        {
            _mediator = mediator;
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// Lấy danh sách các phương thức thông báo và trạng thái bật/tắt
        /// </summary>
        /// <remarks>
        /// Dùng để quản trị các kênh thông báo như Email, SMS, Push trong hệ thống.
        /// </remarks>
        [HttpGet("methods")]
        [ProducesResponseType(typeof(List<NotificationMethodDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNotificationMethods()
        {
            var methods = await _mediator.Send(new GetNotificationMethodsQuery());

            return Ok(ApiResponse<List<NotificationMethodDto>>.SuccessResponse(methods));
        }

        /// <summary>
        /// Bật (subscribe) một phương thức thông báo
        /// </summary>
        /// <remarks>
        /// methodName hỗ trợ: email, sms, push (hoặc emailnotification/smsnotification/pushnotification).
        /// </remarks>
        [HttpPost("methods/{methodName}/subscribe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SubscribeMethod(string methodName)
        {
            var result = await _mediator.Send(new SubscribeNotificationMethodCommand
            {
                MethodName = methodName
            });

            if (!result)
            {
                return NotFound(ApiResponse<bool>.FailureResponse("Không tìm thấy phương thức thông báo"));
            }

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Đã bật phương thức thông báo"));
        }

        /// <summary>
        /// Tắt (unsubscribe) một phương thức thông báo
        /// </summary>
        /// <remarks>
        /// methodName hỗ trợ: email, sms, push (hoặc emailnotification/smsnotification/pushnotification).
        /// </remarks>
        [HttpDelete("methods/{methodName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UnsubscribeMethod(string methodName)
        {
            var result = await _mediator.Send(new UnsubscribeNotificationMethodCommand
            {
                MethodName = methodName
            });

            if (!result)
            {
                return NotFound(ApiResponse<bool>.FailureResponse("Không tìm thấy phương thức thông báo"));
            }

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Đã tắt phương thức thông báo"));
        }

        /// <summary>
        /// Lấy danh sách các observer đang đăng ký
        /// Observer Pattern - Hiển thị tất cả subscribers
        /// </summary>
        [HttpGet("subscribers")]
        [ProducesResponseType(typeof(SubscribersResponseDto), StatusCodes.Status200OK)]
        public IActionResult GetSubscribers()
        {
            var count = _eventPublisher.GetSubscriberCount();
            var names = _eventPublisher.GetSubscriberNames();

            var response = new SubscribersResponseDto
            {
                TotalSubscribers = count,
                Subscribers = names,
                Message = $"{count} observer đang lắng nghe các sự kiện"
            };

            return Ok(ApiResponse<SubscribersResponseDto>.SuccessResponse(response));
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

    }
}
