using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Application.Common.DTOs.Common;
using mtkpm.Application.Features.Products.Commands.CalculatePrice;
using mtkpm.Application.Common.Interfaces.Services;

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
        /// Tính giá sản phẩm sử dụng Strategy Pattern
        /// Có thể chỉ định strategy hoặc để hệ thống tự chọn giá tốt nhất
        /// </summary>
        /// <remarks>
        /// Các tùy chọn strategy: "regular", "bulk", "seasonal", "vip"
        /// Nếu không chỉ định, sẽ tự động chọn giá tốt nhất
        /// </remarks>
        [HttpPost("calculate")]
        [ProducesResponseType(typeof(CalculatePriceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CalculatePrice([FromBody] CalculatePriceCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(ApiResponse<CalculatePriceResponse>.SuccessResponse(result, "Tính giá thành công"));
        }

        /// <summary>
        /// Lấy danh sách tất cả các chiến lược định giá có sẵn
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
                    DisplayName = "Giá thường",
                    Description = "Giá bán thường không có chiết khấu"
                },
                new PricingStrategyInfo 
                { 
                    Name = "bulk", 
                    DisplayName = "Chiết khấu số lượng",
                    Description = "Giảm giá khi mua 10+ sản phẩm (giảm 10%)"
                },
                new PricingStrategyInfo 
                { 
                    Name = "seasonal", 
                    DisplayName = "Giá mùa vụ",
                    Description = "Giá đặc biệt theo mùa/dịp lễ (Black Friday, Tết, v.v.)"
                },
                new PricingStrategyInfo 
                { 
                    Name = "vip", 
                    DisplayName = "Giá thành viên VIP",
                    Description = "Giá đặc biệt cho thành viên VIP (Bronze 5%, Silver 10%, Gold 15%, Platinum 25%)"
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
