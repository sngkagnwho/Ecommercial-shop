using MediatR;

namespace mtkpm.Application.Features.Orders.Commands.MarkAsPaid
{
    public class MarkAsPaidCommand : IRequest<bool>
    {
        public int OrderId { get; set; }

        public MarkAsPaidCommand(int orderId)
        {
            OrderId = orderId;
        }
    }
}
