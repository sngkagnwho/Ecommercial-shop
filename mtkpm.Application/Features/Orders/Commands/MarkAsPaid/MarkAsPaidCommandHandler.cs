using MediatR;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Orders.Commands.MarkAsPaid
{
    public class MarkAsPaidCommandHandler : IRequestHandler<MarkAsPaidCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public MarkAsPaidCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(MarkAsPaidCommand request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {request.OrderId} not found");
            }

            if (order.IsPaid)
            {
                throw new InvalidOperationException("Order is already marked as paid");
            }

            order.MarkAsPaid();
            
            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
