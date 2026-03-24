using MediatR;
using mtkpm.Application.Common.DTOs.Order;

namespace mtkpm.Application.Features.Orders.Queries.GetOrderByNumber
{
    public class GetOrderByNumberQuery : IRequest<OrderDto?>
    {
        public string OrderNumber { get; set; }

        public GetOrderByNumberQuery(string orderNumber)
        {
            OrderNumber = orderNumber;
        }
    }
}
