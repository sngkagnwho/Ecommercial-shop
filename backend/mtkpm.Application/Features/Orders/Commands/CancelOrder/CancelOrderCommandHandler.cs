using MediatR;
using mtkpm.Application.Common.Interfaces.Repositories;
using mtkpm.Domain.Enums.Business;

namespace mtkpm.Application.Features.Orders.Commands.CancelOrder
{
    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CancelOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Orders.GetWithDetailsAsync(request.OrderId, cancellationToken);
            if (order == null)
            {
                throw new KeyNotFoundException($"??n hàng có ID {request.OrderId} không t?n t?i");
            }

            if (order.UserId != request.UserId)
            {
                throw new UnauthorizedAccessException("B?n không có quy?n h?y ??n hàng này");
            }

            if (order.Status == OrderStatus.Shipping || order.Status == OrderStatus.Delivered || order.Status == OrderStatus.Completed)
            {
                throw new InvalidOperationException($"Không th? h?y ??n hàng có tr?ng thái: {order.Status}");
            }

            foreach (var item in order.OrderItems)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.IncreaseStock(item.Quantity);
                    _unitOfWork.Products.Update(product);
                }
            }

            order.UpdateStatus(OrderStatus.Cancelled);
            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
