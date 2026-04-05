using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Application.Common.DTOs.Common;
using mtkpm.Application.Common.DTOs.Pricing;
using mtkpm.Application.Features.PricingRules.Commands.CreatePricingRule;
using mtkpm.Application.Features.PricingRules.Commands.DeletePricingRule;
using mtkpm.Application.Features.PricingRules.Commands.UpdatePricingRule;
using mtkpm.Application.Features.PricingRules.Queries.GetPricingRuleById;
using mtkpm.Application.Features.PricingRules.Queries.GetPricingRules;
using mtkpm.Application.Features.Products.Commands.CalculatePrice;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Infrastructure.Services;

namespace mtkpm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class PricingController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public PricingController(
            IMediator mediator,
            ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
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
        [AllowAnonymous]
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
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<PricingRuleDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPricingStrategies()
        {
            var strategyDtos = await _mediator.Send(new GetPricingRulesQuery());

            return Ok(ApiResponse<List<PricingRuleDto>>.SuccessResponse(strategyDtos));
        }

        /// <summary>
        /// Lấy chi tiết một quy tắc định giá theo Id
        /// </summary>
        /// <remarks>
        /// Dùng để xem cấu hình đầy đủ của một pricing rule trước khi chỉnh sửa.
        /// </remarks>
        [HttpGet("strategies/{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PricingRuleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPricingStrategyById(int id)
        {
            var dto = await _mediator.Send(new GetPricingRuleByIdQuery(id));
            if (dto == null)
            {
                return NotFound(ApiResponse<PricingRuleDto>.FailureResponse("Không tìm thấy quy tắc định giá"));
            }

            return Ok(ApiResponse<PricingRuleDto>.SuccessResponse(dto));
        }

        /// <summary>
        /// Tạo mới quy tắc định giá
        /// </summary>
        /// <remarks>
        /// Chỉ Admin được phép tạo. Quy tắc mới sẽ được hệ thống dùng trong flow tính giá.
        /// </remarks>
        [HttpPost("strategies")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PricingRuleDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePricingStrategy([FromBody] CreatePricingRuleDto dto)
        {
            var result = await _mediator.Send(new CreatePricingRuleCommand
            {
                Dto = dto,
                UserId = _currentUserService.UserId
            });

            return CreatedAtAction(nameof(GetPricingStrategyById), new { id = result.Id }, ApiResponse<PricingRuleDto>.SuccessResponse(result, "Tạo quy tắc định giá thành công"));
        }

        /// <summary>
        /// Cập nhật quy tắc định giá theo Id
        /// </summary>
        /// <remarks>
        /// Chỉ Admin được phép cập nhật. Dùng để thay đổi điều kiện, giá trị rule, thời gian hiệu lực.
        /// </remarks>
        [HttpPut("strategies/{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PricingRuleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePricingStrategy(int id, [FromBody] UpdatePricingRuleDto dto)
        {
            var result = await _mediator.Send(new UpdatePricingRuleCommand
            {
                Id = id,
                Dto = dto,
                UserId = _currentUserService.UserId
            });

            if (result == null)
            {
                return NotFound(ApiResponse<PricingRuleDto>.FailureResponse("Không tìm thấy quy tắc định giá"));
            }

            return Ok(ApiResponse<PricingRuleDto>.SuccessResponse(result, "Cập nhật quy tắc định giá thành công"));
        }

        /// <summary>
        /// Xóa mềm một quy tắc định giá theo Id
        /// </summary>
        /// <remarks>
        /// Chỉ Admin được phép xóa. Hệ thống dùng soft delete để giữ lịch sử dữ liệu.
        /// </remarks>
        [HttpDelete("strategies/{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePricingStrategy(int id)
        {
            var deleted = await _mediator.Send(new DeletePricingRuleCommand
            {
                Id = id,
                UserId = _currentUserService.UserId
            });

            if (!deleted)
            {
                return NotFound(ApiResponse<bool>.FailureResponse("Không tìm thấy quy tắc định giá"));
            }

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Xóa quy tắc định giá thành công"));
        }
    }

    public class PricingStrategyInfo
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
}
