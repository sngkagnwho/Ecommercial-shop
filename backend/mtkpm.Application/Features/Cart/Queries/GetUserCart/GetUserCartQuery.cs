using MediatR;
using mtkpm.Application.Common.DTOs.Cart;

namespace mtkpm.Application.Features.Cart.Queries.GetUserCart
{
    public class GetUserCartQuery : IRequest<CartDto>
    {
        public int UserId { get; set; }

        public GetUserCartQuery(int userId)
        {
            UserId = userId;
        }
    }
}
