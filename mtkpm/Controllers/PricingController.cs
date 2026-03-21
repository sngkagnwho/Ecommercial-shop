using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Application.Common.DTOs.Common;
using mtkpm.Application.Features.Products.Commands.CalculatePrice;
using mtkpm.Infrastructure.Services;

namespace mtkpm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [AllowAnonymous]
    public class PricingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PricingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Tính giá s?n ph?m dùng Strategy Pattern
        /// Có th? ch? ??nh strategy ho?c ?? h? th?ng t? ch?n giá t?t nh?t
        /// </summary>
        /// <remarks>
        /// Strategy options: "regular", "bulk", "seasonal", "vip"
        /// N?u không ch? ??nh, s? t? ??ng ch?n giá t?t nh?t
        /// </remarks>
        [HttpPost("calculate")]
        [ProducesResponseType(typeof(CalculatePriceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CalculatePrice([FromBody] CalculatePriceCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(ApiResponse<CalculatePriceResponse>.SuccessResponse(result, "Calculated pricing successfully"));
        }

        /// <summary>
        /// L?y danh sách t?t c? pricing strategies có s?n
        /// </summary>
        [HttpGet("strategies")]
        [ProducesResponseType(typeof(List<PricingStrategyInfo>), StatusCodes.Status200OK)]
        public IActionResult GetPricingStrategies()
        {
            var strategies = new List<PricingStrategyInfo>
            {
                new PricingStrategyInfo 
                { 
                    Name = "regular", 
                    DisplayName = "Regular Pricing",
                    Description = "Giá bán th??ng không có chi?t kh?u"
                },
                new PricingStrategyInfo 
                { 
                    Name = "bulk", 
                    DisplayName = "Bulk Discount",
                    Description = "Gi?m giá khi mua 10+ s?n ph?m (10% discount)"
                },
                new PricingStrategyInfo 
                { 
                    Name = "seasonal", 
                    DisplayName = "Seasonal Pricing",
                    Description = "Giá ??c bi?t theo mùa/d?p l? (Black Friday, T?t, etc)"
                },
                new PricingStrategyInfo 
                { 
                    Name = "vip", 
                    DisplayName = "VIP Member Pricing",
                    Description = "Giá ??c bi?t cho thành viên VIP (Bronze 5%, Silver 10%, Gold 15%, Platinum 25%)"
                }
            };

            return Ok(ApiResponse<List<PricingStrategyInfo>>.SuccessResponse(strategies));
        }
    }

    public class PricingStrategyInfo
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
}
