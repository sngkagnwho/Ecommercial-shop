using MediatR;

namespace mtkpm.Application.Features.Cart.Queries.GetCartItemCount
{
    public class GetCartItemCountQuery : IRequest<int>
    {
        public int UserId { get; set; }

        public GetCartItemCountQuery(int userId)
        {
            UserId = userId;
        }
    }
}
