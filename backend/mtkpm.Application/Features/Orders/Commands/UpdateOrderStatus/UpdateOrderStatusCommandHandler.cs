using MediatR;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Orders.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {request.OrderId} not found");
            }

            order.UpdateStatus(request.Status);
            
            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
