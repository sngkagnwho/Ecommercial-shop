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
        /// Lấy danh sách các phương thức thanh toán có sẵn
        /// </summary>
        [HttpGet("methods")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<PaymentMethodInfo>), StatusCodes.Status200OK)]
        public IActionResult GetPaymentMethods()
        {
            var methods = new List<PaymentMethodInfo>
            {
                new PaymentMethodInfo
                {
                    Code = "CreditCard",
                    Name = "Thẻ Tín Dụng",
                    Description = "Thanh toán bằng thẻ tín dụng (Visa, Mastercard, v.v.)",
                    Icon = "💳",
                    IsActive = true,
                    Fee = 0m // Không có phí
                },
                new PaymentMethodInfo
                {
                    Code = "BankTransfer",
                    Name = "Chuyển Khoản Ngân Hàng",
                    Description = "Thanh toán bằng chuyển khoản ngân hàng trực tiếp",
                    Icon = "🏦",
                    IsActive = true,
                    Fee = 0m
                },
                new PaymentMethodInfo
                {
                    Code = "COD",
                    Name = "Thanh Toán Khi Nhận Hàng (COD)",
                    Description = "Thanh toán khi nhận hàng, không cần trả tiền trước",
                    Icon = "📦",
                    IsActive = true,
                    Fee = 0m
                }
            };

            return Ok(ApiResponse<List<PaymentMethodInfo>>.SuccessResponse(
                methods, 
                "Danh sách phương thức thanh toán"
            ));
        }

        /// <summary>
        /// Lấy chi tiết phương thức thanh toán theo code
        /// </summary>
        [HttpGet("methods/{code}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PaymentMethodDetail), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPaymentMethodDetail(string code)
        {
            var details = new Dictionary<string, PaymentMethodDetail>
            {
                {
                    "CreditCard",
                    new PaymentMethodDetail
                    {
                        Code = "CreditCard",
                        Name = "Thẻ Tín Dụng",
                        Description = "Thanh toán an toàn bằng thẻ tín dụng",
                        ProcessingTime = "Tức thời",
                        Requirements = new List<string>
                        {
                            "Số thẻ tín dụng",
                            "Tên chủ thẻ",
                            "Ngày hết hạn",
                            "CVV"
                        },
                        SupportedCards = new List<string> { "Visa", "Mastercard", "American Express" },
                        Fee = 0m,
                        MinAmount = 10000m,
                        MaxAmount = 1000000000m
                    }
                },
                {
                    "BankTransfer",
                    new PaymentMethodDetail
                    {
                        Code = "BankTransfer",
                        Name = "Chuyển Khoản Ngân Hàng",
                        Description = "Chuyển khoản từ ngân hàng của bạn đến ngân hàng của chúng tôi",
                        ProcessingTime = "1-3 ngày làm việc",
                        Requirements = new List<string>
                        {
                            "Tên ngân hàng",
                            "Số tài khoản nhận",
                            "Mô tả chuyển khoản (Mã đơn hàng)"
                        },
                        SupportedBanks = new List<string>
                        {
                            "Vietcombank",
                            "Techcombank",
                            "BIDV",
                            "VP Bank",
                            "ACB",
                            "Các ngân hàng khác"
                        },
                        Fee = 0m,
                        MinAmount = 50000m,
                        MaxAmount = 5000000000m
                    }
                },
                {
                    "COD",
                    new PaymentMethodDetail
                    {
                        Code = "COD",
                        Name = "Thanh Toán Khi Nhận Hàng",
                        Description = "Bạn chỉ thanh toán khi đã kiểm tra và nhận hàng",
                        ProcessingTime = "Khi giao hàng",
                        Requirements = new List<string>
                        {
                            "Địa chỉ giao hàng chính xác",
                            "Số điện thoại liên hệ"
                        },
                        AvailableAreas = new List<string>
                        {
                            "Toàn thành phố Hồ Chí Minh",
                            "Toàn tỉnh Bình Dương",
                            "Toàn tỉnh Đồng Nai",
                            "Các tỉnh khác (phí giao hàng tăng)"
                        },
                        Fee = 0m,
                        MinAmount = 10000m,
                        MaxAmount = 10000000m
                    }
                }
            };

            if (!details.TryGetValue(code, out var detail))
            {
                return NotFound(ApiResponse<PaymentMethodDetail>.FailureResponse("Phương thức thanh toán không tìm thấy"));
            }

            return Ok(ApiResponse<PaymentMethodDetail>.SuccessResponse(detail));
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
        [ProducesResponseType(typeof(PaymentStatusInfo), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPaymentStatus(int orderId)
        {
            // TODO: Implement logic to get payment status from database
            var status = new PaymentStatusInfo
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

            return Ok(ApiResponse<PaymentStatusInfo>.SuccessResponse(status));
        }
    }

    public class ProcessPaymentRequest
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethodType PaymentMethod { get; set; }
    }

    public class PaymentMethodInfo
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }
        public decimal Fee { get; set; }
    }

    public class PaymentMethodDetail
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProcessingTime { get; set; }
        public List<string> Requirements { get; set; } = new();
        public List<string> SupportedCards { get; set; } = new();
        public List<string> SupportedBanks { get; set; } = new();
        public List<string> AvailableAreas { get; set; } = new();
        public decimal Fee { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
    }

    public class PaymentStatusInfo
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public string TransactionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Message { get; set; }
    }
}
