using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.Order;
using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Repositories;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public CreateOrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILoggerService logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderNumber = GenerateOrderNumber();
            decimal subTotal = 0;

            var order = new Order(
                userId: request.UserId,
                orderNumber: orderNumber,
                shippingAddress: request.ShippingAddress,
                billingAddress: request.BillingAddress,
                subTotal: 0,
                shippingFee: 30000,
                discount: 0,
                paymentMethod: request.PaymentMethod,
                note: request.Note
            );

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInfo($"T?o ??n hàng m?i: OrderId={order.Id}, UserId={request.UserId}", "OrderService");

            foreach (var item in request.OrderItems)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with ID {item.ProductId} not found");
                }

                if (product.StockQuantity < item.Quantity)
                {
                    throw new InvalidOperationException($"Not enough stock for product '{product.Name}'. Available: {product.StockQuantity}, Requested: {item.Quantity}");
                }

                var orderItem = new OrderItem(
                    orderId: order.Id,
                    productId: product.Id,
                    productName: product.Name,
                    quantity: item.Quantity,
                    priceAtOrder: product.Price
                );

                order.AddOrderItem(orderItem);
                product.DecreaseStock(item.Quantity);
                _logger.LogInfo($"Tr? kho s?n ph?m Id={product.Id}, S? l??ng={item.Quantity}", "ProductService");
                _unitOfWork.Products.Update(product);
            }

            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInfo($"Hoàn t?t ??n hàng: OrderId={order.Id}, T?ng ti?n={order.TotalAmount}", "OrderService");

            var orderDto = await _unitOfWork.Orders.GetWithDetailsAsync(order.Id, cancellationToken);
            return _mapper.Map<OrderDto>(orderDto);
        }

        private string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper()}";
        }
    }
}
