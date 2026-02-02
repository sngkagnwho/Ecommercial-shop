using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Application.Common.DTOs.Common;
using mtkpm.Application.Common.DTOs.Order;
using mtkpm.Application.Features.Orders.Commands.CancelOrder;
using mtkpm.Application.Features.Orders.Commands.CreateOrder;
using mtkpm.Application.Features.Orders.Commands.MarkAsPaid;
using mtkpm.Application.Features.Orders.Commands.UpdateOrderStatus;
using mtkpm.Application.Features.Orders.Queries.GetOrderById;
using mtkpm.Application.Features.Orders.Queries.GetOrderByNumber;
using mtkpm.Application.Features.Orders.Queries.GetUserOrders;
using mtkpm.Domain.Enums.Business;
using mtkpm.Infrastructure.Services;

namespace mtkpm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public OrdersController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Lấy danh sách đơn hàng của user
        /// </summary>
        [HttpGet("my-orders")]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = _currentUserService.UserId!.Value;
            var query = new GetUserOrdersQuery(userId);
            var result = await _mediator.Send(query);
            return Ok(ApiResponse<IEnumerable<OrderDto>>.SuccessResponse(result));
        }

        /// <summary>
        /// Lấy đơn hàng theo ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var query = new GetOrderByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(ApiResponse<OrderDto>.FailureResponse("Không tìm thấy đơn hàng"));
            }

            var userId = _currentUserService.UserId!.Value;
            if (result.UserId != userId && !_currentUserService.IsInRole("Admin"))
            {
                return Forbid();
            }

            return Ok(ApiResponse<OrderDto>.SuccessResponse(result));
        }

        /// <summary>
        /// Lấy đơn hàng theo số đơn hàng
        /// </summary>
        [HttpGet("number/{orderNumber}")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderByNumber(string orderNumber)
        {
            var query = new GetOrderByNumberQuery(orderNumber);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(ApiResponse<OrderDto>.FailureResponse("Không tìm thấy đơn hàng"));
            }

            var userId = _currentUserService.UserId!.Value;
            if (result.UserId != userId && !_currentUserService.IsInRole("Admin"))
            {
                return Forbid();
            }

            return Ok(ApiResponse<OrderDto>.SuccessResponse(result));
        }

        /// <summary>
        /// Tạo đơn hàng mới
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            var userId = _currentUserService.UserId!.Value;
            var command = new CreateOrderCommand
            {
                UserId = userId,
                ShippingAddress = dto.ShippingAddress,
                BillingAddress = dto.BillingAddress,
                PaymentMethod = dto.PaymentMethod,
                Note = dto.Note,
                OrderItems = dto.OrderItems.Select(x => new Application.Features.Orders.Commands.CreateOrder.CreateOrderItemDto
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToList()
            };

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetOrderById), new { id = result.Id }, ApiResponse<OrderDto>.SuccessResponse(result, "Đặt hàng thành công"));
        }

        /// <summary>
        /// Hủy đơn hàng
        /// </summary>
        [HttpPost("{id}/cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var userId = _currentUserService.UserId!.Value;
            var command = new CancelOrderCommand(id, userId);
            await _mediator.Send(command);
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Hủy đơn hàng thành công"));
        }

        /// <summary>
        /// Cập nhật trạng thái đơn hàng (Admin only)
        /// </summary>
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto dto)
        {
            var command = new UpdateOrderStatusCommand(id, dto.Status);
            await _mediator.Send(command);
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Cập nhật trạng thái thành công"));
        }

        /// <summary>
        /// Đánh dấu đơn hàng đã thanh toán (Admin only)
        /// </summary>
        [HttpPost("{id}/mark-paid")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            var command = new MarkAsPaidCommand(id);
            await _mediator.Send(command);
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Đánh dấu đã thanh toán"));
        }
    }
}
