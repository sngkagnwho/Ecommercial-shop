using MediatR;
using MediatR;
using mtkpm.Application.Common.DTOs.Order;

namespace mtkpm.Application.Features.Orders.Queries.GetAllOrders
{
    public class GetAllOrdersQuery : IRequest<IEnumerable<OrderDto>>
    {
    }
}
