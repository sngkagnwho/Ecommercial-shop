using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Application.Common.DTOs.Common;
using mtkpm.Application.Features.Cart.Commands.CalculateDiscount;
using mtkpm.Infrastructure.Services;

namespace mtkpm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class DiscountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public DiscountController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Tính giá giỏ hàng sau khi áp dụng chiết khấu
        /// Sử dụng Decorator Pattern để stack multiple discounts
        /// </summary>
        /// <remarks>
        /// Mã chiết khấu ví dụ:
        /// - "percentage_10" = giảm 10%
        /// - "fixed_100000" = giảm 100.000 đ
        /// - "free_shipping" = miễn phí vận chuyển
        /// - "loyalty_points_50" = sử dụng 50 điểm thành viên
        /// - "bundle_3_15" = mua 3+ sản phẩm được -15%
        /// 
        /// Ví dụ body:
        /// {
        ///   "discountCodes": ["percentage_10", "free_shipping"]
        /// }
        /// </remarks>
        [HttpPost("calculate")]
        [ProducesResponseType(typeof(CalculateCartDiscountResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CalculateDiscount([FromBody] CalculateCartDiscountRequest request)
        {
            var userId = _currentUserService.UserId ?? 0;
            
            var command = new CalculateCartDiscountCommand
            {
                UserId = userId,
                DiscountCodes = request.DiscountCodes
            };

            var result = await _mediator.Send(command);
            return Ok(ApiResponse<CalculateCartDiscountResponse>.SuccessResponse(result, "Tính chiết khấu thành công"));
        }

        /// <summary>
        /// Lấy danh sách các mã chiết khấu có sẵn
        /// </summary>
        [HttpGet("available")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<DiscountCodeInfo>), StatusCodes.Status200OK)]
        public IActionResult GetAvailableDiscounts()
        {
            var discountCodes = new List<DiscountCodeInfo>
            {
                new DiscountCodeInfo
                {
                    Code = "percentage_10",
                    Name = "Giảm 10%",
                    Description = "Giảm 10% trên giá hàng",
                    Example = "percentage_10"
                },
                new DiscountCodeInfo
                {
                    Code = "percentage_20",
                    Name = "Giảm 20%",
                    Description = "Giảm 20% trên giá hàng",
                    Example = "percentage_20"
                },
                new DiscountCodeInfo
                {
                    Code = "fixed_100000",
                    Name = "Giảm 100K",
                    Description = "Giảm 100.000 đ cố định",
                    Example = "fixed_100000"
                },
                new DiscountCodeInfo
                {
                    Code = "free_shipping",
                    Name = "Miễn phí vận chuyển",
                    Description = "Miễn phí vận chuyển (tiết kiệm 50.000 đ)",
                    Example = "free_shipping"
                },
                new DiscountCodeInfo
                {
                    Code = "loyalty_points_50",
                    Name = "50 điểm thành viên",
                    Description = "Sử dụng 50 điểm thành viên (50.000 đ)",
                    Example = "loyalty_points_50"
                },
                new DiscountCodeInfo
                {
                    Code = "bundle_3_15",
                    Name = "Chiết khấu combo (3+ sản phẩm -15%)",
                    Description = "Mua 3+ sản phẩm được giảm 15%",
                    Example = "bundle_3_15"
                }
            };

            return Ok(ApiResponse<List<DiscountCodeInfo>>.SuccessResponse(discountCodes));
        }

        /// <summary>
        /// Hướng dẫn sử dụng Decorator Pattern
        /// </summary>
        [HttpGet("guide")]
        [AllowAnonymous]
        public IActionResult GetDecoratorPatternGuide()
        {
            var guide = @"
Decorator Pattern - Hệ thống Chiết khấu

**Mục đích:**
Áp dụng nhiều chiết khấu lần lượt (stacking) mà không cần sửa code.

**Cấu Trúc:**
- BaseDiscount: Component cơ bản (không có chiết khấu)
- DiscountDecorator: Base class cho tất cả decorators
- PercentageDiscountDecorator: Chiết khấu theo phần trăm
- FixedAmountDiscountDecorator: Chiết khấu số tiền cố định
- FreeShippingDiscountDecorator: Miễn phí vận chuyển
- LoyaltyPointsDiscountDecorator: Sử dụng điểm
- BundleDiscountDecorator: Chiết khấu combo

**Ví Dụ Sử Dụng:**

1. Áp dụng một chiết khấu:
   POST /api/discount/calculate
   {
     ""discountCodes"": [""percentage_10""]
   }

2. Stack nhiều chiết khấu:
   POST /api/discount/calculate
   {
     ""discountCodes"": [""percentage_10"", ""free_shipping""]
   }
   
   Kết quả: Giảm 10% + Miễn phí ship = Tiết kiệm cùng lúc

3. Sử dụng điểm + Chiết khấu:
   POST /api/discount/calculate
   {
     ""discountCodes"": [""percentage_20"", ""loyalty_points_100""]
   }

**Lợi Ích Decorator Pattern:**
- Linh hoạt: Có thể kết hợp bất kỳ chiết khấu nào
- Open/Closed: Dễ thêm chiết khấu mới
- Không bùng nổ class: Không cần tạo nhiều class
- Runtime Composition: Quyết định chiết khấu lúc runtime
";

            return Ok(new { guide });
        }
    }

    public class CalculateCartDiscountRequest
    {
        public List<string> DiscountCodes { get; set; } = new();
    }

    public class DiscountCodeInfo
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Example { get; set; }
    }
}
