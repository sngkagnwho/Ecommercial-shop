using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Application.Common.DTOs.Common;
using mtkpm.Application.Features.Orders.Commands.ProcessPayment;
using mtkpm.Infrastructure.Services;
using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Enums.Business;

namespace mtkpm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public PaymentController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Xử lý thanh toán đơn hàng
        /// Sử dụng Factory Pattern - Các phương thức thanh toán được tạo đúng dựa trên loại
        /// </summary>
        [HttpPost("process")]
        [ProducesResponseType(typeof(ProcessPaymentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentRequest request)
        {
            var command = new ProcessPaymentCommand
            {
                OrderId = request.OrderId,
                Amount = request.Amount,
                PaymentMethod = request.PaymentMethod
            };

            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(ApiResponse<ProcessPaymentResponse>.FailureResponse(result.Message));
            }

            return Ok(ApiResponse<ProcessPaymentResponse>.SuccessResponse(result, "Thanh toán đã được xử lý thành công"));
        }
    }

    public class ProcessPaymentRequest
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethodType PaymentMethod { get; set; }
    }
}
