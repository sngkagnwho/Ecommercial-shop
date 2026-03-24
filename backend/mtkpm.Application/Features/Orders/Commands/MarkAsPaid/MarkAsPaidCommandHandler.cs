using MediatR;
using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Orders.Commands.MarkAsPaid
{
    public class MarkAsPaidCommandHandler : IRequestHandler<MarkAsPaidCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;

        public MarkAsPaidCommandHandler(IUnitOfWork unitOfWork, ILoggerService logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
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
            _logger.LogInfo($"Thanh toán thành công: OrderId={order.Id}, UserId={order.UserId}", "PaymentService");
            
            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
