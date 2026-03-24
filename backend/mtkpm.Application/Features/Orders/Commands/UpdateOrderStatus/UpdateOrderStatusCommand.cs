using MediatR;
using mtkpm.Domain.Enums.Business;

namespace mtkpm.Application.Features.Orders.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommand : IRequest<bool>
    {
        public int OrderId { get; set; }
        public OrderStatus Status { get; set; }

        public UpdateOrderStatusCommand(int orderId, OrderStatus status)
        {
            OrderId = orderId;
            Status = status;
        }
    }
}
