using MediatR;
using mtkpm.Application.Common.DTOs.Order;

namespace mtkpm.Application.Features.Orders.Queries.GetUserOrders
{
    public class GetUserOrdersQuery : IRequest<IEnumerable<OrderDto>>
    {
        public int UserId { get; set; }

        public GetUserOrdersQuery(int userId)
        {
            UserId = userId;
        }
    }
}
