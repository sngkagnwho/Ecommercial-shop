using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Application.Common.DTOs.Common;
using mtkpm.Application.Common.DTOs.Payment;
using mtkpm.Application.Features.PaymentMethodConfigs.Commands.CreatePaymentMethodConfig;
using mtkpm.Application.Features.PaymentMethodConfigs.Commands.DeletePaymentMethodConfig;
using mtkpm.Application.Features.PaymentMethodConfigs.Commands.UpdatePaymentMethodConfig;
using mtkpm.Application.Features.PaymentMethodConfigs.Queries.GetPaymentMethodConfigByCode;
using mtkpm.Application.Features.PaymentMethodConfigs.Queries.GetPaymentMethodConfigs;
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

        public PaymentController(
            IMediator mediator,
            ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Lấy danh sách các phương thức thanh toán có sẵn
        /// </summary>
        [HttpGet("methods")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<PaymentMethodConfigDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaymentMethods()
        {
            var methodDtos = await _mediator.Send(new GetPaymentMethodConfigsQuery());

            return Ok(ApiResponse<List<PaymentMethodConfigDto>>.SuccessResponse(
                methodDtos,
                "Danh sách phương thức thanh toán"
            ));
        }

        /// <summary>
        /// Lấy chi tiết phương thức thanh toán theo mã
        /// </summary>
        [HttpGet("methods/{code}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PaymentMethodConfigDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPaymentMethodDetail(string code)
        {
            var dto = await _mediator.Send(new GetPaymentMethodConfigByCodeQuery(code));
            if (dto == null)
            {
                return NotFound(ApiResponse<PaymentMethodConfigDto>.FailureResponse("Phương thức thanh toán không tìm thấy"));
            }

            return Ok(ApiResponse<PaymentMethodConfigDto>.SuccessResponse(dto));
        }

        /// <summary>
        /// Tạo mới một phương thức thanh toán
        /// </summary>
        /// <remarks>
        /// Chỉ Admin được phép tạo. Dữ liệu sẽ được lưu vào bảng PaymentMethodConfigs.
        /// </remarks>
        [HttpPost("methods")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PaymentMethodConfigDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePaymentMethod([FromBody] CreatePaymentMethodConfigDto dto)
        {
            var result = await _mediator.Send(new CreatePaymentMethodConfigCommand
            {
                Dto = dto,
                UserId = _currentUserService.UserId
            });

            return CreatedAtAction(nameof(GetPaymentMethodDetail), new { code = result.Code }, ApiResponse<PaymentMethodConfigDto>.SuccessResponse(result, "Tạo phương thức thanh toán thành công"));
        }

        /// <summary>
        /// Cập nhật thông tin phương thức thanh toán theo Id
        /// </summary>
        /// <remarks>
        /// Chỉ Admin được phép cập nhật. Dùng để đổi tên, phí, trạng thái hoạt động, giới hạn số tiền...
        /// </remarks>
        [HttpPut("methods/{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PaymentMethodConfigDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePaymentMethod(int id, [FromBody] UpdatePaymentMethodConfigDto dto)
        {
            var result = await _mediator.Send(new UpdatePaymentMethodConfigCommand
            {
                Id = id,
                Dto = dto,
                UserId = _currentUserService.UserId
            });

            if (result == null)
            {
                return NotFound(ApiResponse<PaymentMethodConfigDto>.FailureResponse("Không tìm thấy phương thức thanh toán"));
            }

            return Ok(ApiResponse<PaymentMethodConfigDto>.SuccessResponse(result, "Cập nhật phương thức thanh toán thành công"));
        }

        /// <summary>
        /// Xóa mềm một phương thức thanh toán theo Id
        /// </summary>
        /// <remarks>
        /// Chỉ Admin được phép xóa. Dữ liệu không bị mất vật lý, hệ thống dùng soft delete.
        /// </remarks>
        [HttpDelete("methods/{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePaymentMethod(int id)
        {
            var deleted = await _mediator.Send(new DeletePaymentMethodConfigCommand
            {
                Id = id,
                UserId = _currentUserService.UserId
            });

            if (!deleted)
            {
                return NotFound(ApiResponse<bool>.FailureResponse("Không tìm thấy phương thức thanh toán"));
            }

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Xóa phương thức thanh toán thành công"));
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

        /// <summary>
        /// Kiểm tra trạng thái thanh toán của đơn hàng
        /// </summary>
        [HttpGet("status/{orderId}")]
        [ProducesResponseType(typeof(PaymentStatusInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPaymentStatus(int orderId)
        {
            // TODO: Implement logic to get payment status from database
            var status = new PaymentStatusInfoDto
            {
                OrderId = orderId,
                Status = "Pending",
                PaymentMethod = "CreditCard",
                Amount = 1000000m,
                TransactionId = "TXN-" + orderId,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow,
                Message = "Thanh toán đang chờ xác nhận"
            };

            return Ok(ApiResponse<PaymentStatusInfoDto>.SuccessResponse(status));
        }
    }

    public class ProcessPaymentRequest
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethodType PaymentMethod { get; set; }
    }

}
