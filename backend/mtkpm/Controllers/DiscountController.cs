using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Application.Common.DTOs.Common;
using mtkpm.Application.Common.DTOs.Discount;
using mtkpm.Application.Features.Discounts.Commands.CreateDiscount;
using mtkpm.Application.Features.Discounts.Commands.DeleteDiscount;
using mtkpm.Application.Features.Discounts.Commands.UpdateDiscount;
using mtkpm.Application.Features.Discounts.Queries.GetDiscountById;
using mtkpm.Application.Features.Discounts.Queries.GetDiscounts;
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
        public async Task<IActionResult> GetAvailableDiscounts()
        {
            var discounts = await _mediator.Send(new GetDiscountsQuery { IncludeInactive = false });
            var discountCodes = discounts.Select(d => new DiscountCodeInfo
            {
                Code = d.Code,
                Name = d.Name,
                Description = d.Description ?? string.Empty,
                Example = d.Code
            }).ToList();

            return Ok(ApiResponse<List<DiscountCodeInfo>>.SuccessResponse(discountCodes));
        }

        /// <summary>
        /// Lấy danh sách discount cho Admin
        /// </summary>
        /// <remarks>
        /// includeInactive=true để xem cả discount đã tắt/hết hạn, false để chỉ lấy discount đang hoạt động.
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(List<DiscountDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDiscounts([FromQuery] bool includeInactive = true)
        {
            var discounts = await _mediator.Send(new GetDiscountsQuery { IncludeInactive = includeInactive });
            return Ok(ApiResponse<List<DiscountDto>>.SuccessResponse(discounts));
        }

        /// <summary>
        /// Lấy chi tiết một discount theo Id
        /// </summary>
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(DiscountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDiscountById(int id)
        {
            var discount = await _mediator.Send(new GetDiscountByIdQuery(id));
            if (discount == null)
            {
                return NotFound(ApiResponse<DiscountDto>.FailureResponse("Không tìm thấy chiết khấu"));
            }

            return Ok(ApiResponse<DiscountDto>.SuccessResponse(discount));
        }

        /// <summary>
        /// Tạo mới discount
        /// </summary>
        /// <remarks>
        /// Chỉ Admin được phép tạo. Mã discount phải duy nhất trong hệ thống.
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(DiscountDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateDiscount([FromBody] CreateDiscountDto dto)
        {
            var result = await _mediator.Send(new CreateDiscountCommand
            {
                Dto = dto,
                UserId = _currentUserService.UserId
            });

            return CreatedAtAction(nameof(GetDiscountById), new { id = result.Id }, ApiResponse<DiscountDto>.SuccessResponse(result, "Tạo chiết khấu thành công"));
        }

        /// <summary>
        /// Cập nhật discount theo Id
        /// </summary>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(DiscountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDiscount(int id, [FromBody] UpdateDiscountDto dto)
        {
            var result = await _mediator.Send(new UpdateDiscountCommand
            {
                Id = id,
                Dto = dto,
                UserId = _currentUserService.UserId
            });

            if (result == null)
            {
                return NotFound(ApiResponse<DiscountDto>.FailureResponse("Không tìm thấy chiết khấu"));
            }

            return Ok(ApiResponse<DiscountDto>.SuccessResponse(result, "Cập nhật chiết khấu thành công"));
        }

        /// <summary>
        /// Xóa mềm discount theo Id
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDiscount(int id)
        {
            var deleted = await _mediator.Send(new DeleteDiscountCommand
            {
                Id = id,
                UserId = _currentUserService.UserId
            });

            if (!deleted)
            {
                return NotFound(ApiResponse<bool>.FailureResponse("Không tìm thấy chiết khấu"));
            }

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Xóa chiết khấu thành công"));
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
