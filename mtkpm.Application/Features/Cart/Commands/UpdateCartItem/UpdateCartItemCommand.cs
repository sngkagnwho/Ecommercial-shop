using MediatR;
using mtkpm.Application.Common.DTOs.Cart;

namespace mtkpm.Application.Features.Cart.Commands.UpdateCartItem
{
    public class UpdateCartItemCommand : IRequest<CartItemDto>
    {
        public int UserId { get; set; }
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}
