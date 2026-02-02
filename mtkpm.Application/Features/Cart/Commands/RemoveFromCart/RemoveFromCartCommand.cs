using MediatR;

namespace mtkpm.Application.Features.Cart.Commands.RemoveFromCart
{
    public class RemoveFromCartCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public int CartItemId { get; set; }

        public RemoveFromCartCommand(int userId, int cartItemId)
        {
            UserId = userId;
            CartItemId = cartItemId;
        }
    }
}
