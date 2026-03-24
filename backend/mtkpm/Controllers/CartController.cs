using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Application.Common.DTOs.Cart;
using mtkpm.Application.Common.DTOs.Common;
using mtkpm.Application.Features.Cart.Commands.AddToCart;
using mtkpm.Application.Features.Cart.Commands.ClearCart;
using mtkpm.Application.Features.Cart.Commands.RemoveFromCart;
using mtkpm.Application.Features.Cart.Commands.UpdateCartItem;
using mtkpm.Application.Features.Cart.Queries.GetCartItemCount;
using mtkpm.Application.Features.Cart.Queries.GetUserCart;
using mtkpm.Infrastructure.Services;

namespace mtkpm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public CartController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Lấy giỏ hàng của người dùng
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCart()
        {
            var userId = _currentUserService.UserId!.Value;
            var query = new GetUserCartQuery(userId);
            var result = await _mediator.Send(query);
            return Ok(ApiResponse<CartDto>.SuccessResponse(result));
        }

        /// <summary>
        /// Lấy số lượng sản phẩm trong giỏ hàng
        /// </summary>
        [HttpGet("count")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCartItemCount()
        {
            var userId = _currentUserService.UserId!.Value;
            var query = new GetCartItemCountQuery(userId);
            var result = await _mediator.Send(query);
            return Ok(ApiResponse<int>.SuccessResponse(result));
        }

        /// <summary>
        /// Thêm sản phẩm vào giỏ hàng
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CartItemDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto)
        {
            var userId = _currentUserService.UserId!.Value;
            var command = new AddToCartCommand
            {
                UserId = userId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetCart), null, ApiResponse<CartItemDto>.SuccessResponse(result, "Thêm vào giỏ hàng thành công"));
        }

        /// <summary>
        /// Cập nhật số lượng sản phẩm trong giỏ hàng
        /// </summary>
        [HttpPut("{cartItemId}")]
        [ProducesResponseType(typeof(CartItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCartItem(int cartItemId, [FromBody] UpdateCartItemDto dto)
        {
            var userId = _currentUserService.UserId!.Value;
            var command = new UpdateCartItemCommand
            {
                UserId = userId,
                CartItemId = cartItemId,
                Quantity = dto.Quantity
            };

            var result = await _mediator.Send(command);
            return Ok(ApiResponse<CartItemDto>.SuccessResponse(result, "Cập nhật giỏ hàng thành công"));
        }

        /// <summary>
        /// Xóa sản phẩm khỏi giỏ hàng
        /// </summary>
        [HttpDelete("{cartItemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            var userId = _currentUserService.UserId!.Value;
            var command = new RemoveFromCartCommand(userId, cartItemId);
            await _mediator.Send(command);
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Xóa khỏi giỏ hàng thành công"));
        }

        /// <summary>
        /// Xóa toàn bộ giỏ hàng
        /// </summary>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ClearCart()
        {
            var userId = _currentUserService.UserId!.Value;
            var command = new ClearCartCommand(userId);
            await _mediator.Send(command);
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Đã xóa toàn bộ giỏ hàng"));
        }
    }
}
