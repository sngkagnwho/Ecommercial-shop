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
        /// Tính giá gi? hàng sau khi áp d?ng discounts
        /// S? d?ng Decorator Pattern ?? stack multiple discounts
        /// </summary>
        /// <remarks>
        /// Discount codes examples:
        /// - "percentage_10" = 10% discount
        /// - "fixed_100000" = gi?m 100,000 ?
        /// - "free_shipping" = mi?n phí v?n chuy?n
        /// - "loyalty_points_50" = s? d?ng 50 ?i?m thành viên
        /// - "bundle_3_15" = mua 3+ s?n ph?m ???c -15%
        /// 
        /// Ví d? body:
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
            return Ok(ApiResponse<CalculateCartDiscountResponse>.SuccessResponse(result, "Discount calculated successfully"));
        }

        /// <summary>
        /// L?y danh sách discount codes có s?n
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
                    Name = "10% Discount",
                    Description = "Gi?m 10% t?ng gi? hàng",
                    Example = "percentage_10"
                },
                new DiscountCodeInfo
                {
                    Code = "percentage_20",
                    Name = "20% Discount",
                    Description = "Gi?m 20% t?ng gi? hàng",
                    Example = "percentage_20"
                },
                new DiscountCodeInfo
                {
                    Code = "fixed_100000",
                    Name = "100K Fixed Discount",
                    Description = "Gi?m 100,000 ? c? ??nh",
                    Example = "fixed_100000"
                },
                new DiscountCodeInfo
                {
                    Code = "free_shipping",
                    Name = "Free Shipping",
                    Description = "Mi?n phí v?n chuy?n (ti?t ki?m 50,000 ?)",
                    Example = "free_shipping"
                },
                new DiscountCodeInfo
                {
                    Code = "loyalty_points_50",
                    Name = "50 Loyalty Points",
                    Description = "S? d?ng 50 ?i?m thành viên (50,000 ?)",
                    Example = "loyalty_points_50"
                },
                new DiscountCodeInfo
                {
                    Code = "bundle_3_15",
                    Name = "Bundle Discount (3+ items -15%)",
                    Description = "Mua 3+ s?n ph?m ???c gi?m 15%",
                    Example = "bundle_3_15"
                }
            };

            return Ok(ApiResponse<List<DiscountCodeInfo>>.SuccessResponse(discountCodes));
        }

        /// <summary>
        /// L?y h??ng d?n s? d?ng Decorator Pattern
        /// </summary>
        [HttpGet("guide")]
        [AllowAnonymous]
        public IActionResult GetDecoratorPatternGuide()
        {
            var guide = @"
Decorator Pattern - Discount System

**M?c Tiêu:**
Áp d?ng nhi?u discount l?n l??t (stacking) mà không c?n s?a code.

**C?u Trúc:**
- BaseDiscount: Component c? b?n (không discount)
- DiscountDecorator: Base class cho t?t c? decorators
- PercentageDiscountDecorator: Gi?m theo %
- FixedAmountDiscountDecorator: Gi?m s? ti?n c? ??nh
- FreeShippingDiscountDecorator: Mi?n phí ship
- LoyaltyPointsDiscountDecorator: S? d?ng ?i?m
- BundleDiscountDecorator: Mua combo ???c gi?m

**Ví D? S? D?ng:**

1. Áp d?ng 1 discount:
   POST /api/discount/calculate
   {
     ""discountCodes"": [""percentage_10""]
   }

2. Stack multiple discounts:
   POST /api/discount/calculate
   {
     ""discountCodes"": [""percentage_10"", ""free_shipping""]
   }
   
   K?t qu?: Gi?m 10% + Mi?n phí ship = Ti?t ki?m c?ng d?n

3. S? d?ng ?i?m + Discount:
   POST /api/discount/calculate
   {
     ""discountCodes"": [""percentage_20"", ""loyalty_points_100""]
   }

**L?i Ích Decorator Pattern:**
? Flexible - Có th? combine b?t k? discounts nào
? Open/Closed - D? thêm discount m?i
? No Class Explosion - Không c?n t?o quá nhi?u class
? Runtime Composition - Quy?t ??nh discount lúc runtime
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
