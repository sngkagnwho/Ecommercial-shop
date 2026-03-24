using MediatR;

namespace mtkpm.Application.Features.Cart.Commands.ClearCart
{
    public class ClearCartCommand : IRequest<bool>
    {
        public int UserId { get; set; }

        public ClearCartCommand(int userId)
        {
            UserId = userId;
        }
    }
}
