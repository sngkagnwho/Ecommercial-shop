using MediatR;
using mtkpm.Application.Common.DTOs.Cart;

namespace mtkpm.Application.Features.Cart.Commands.AddToCart
{
    public class AddToCartCommand : IRequest<CartItemDto>
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
