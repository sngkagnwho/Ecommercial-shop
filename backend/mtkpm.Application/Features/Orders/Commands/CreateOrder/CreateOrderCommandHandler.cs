using AutoMapper;
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
            // Xác định địa chỉ giao hàng
            string shippingAddress = request.ShippingAddress;

            if (request.SavedAddressId.HasValue)
            {
                var savedAddress = await _unitOfWork.UserAddresses
                    .GetByIdAndUserIdAsync(request.SavedAddressId.Value, request.UserId, cancellationToken);

                if (savedAddress == null)
                {
                    throw new InvalidOperationException("Địa chỉ đã lưu không tìm thấy hoặc không thuộc về người dùng này");
                }

                // Chuyển UserAddress thành chuỗi định dạng để lưu vào Order
                shippingAddress = FormatAddressToString(savedAddress);
            }

            if (string.IsNullOrEmpty(shippingAddress))
            {
                throw new InvalidOperationException("Địa chỉ giao hàng không được để trống");
            }

            var orderNumber = GenerateOrderNumber();
            decimal subTotal = 0;

            var order = new Order(
                userId: request.UserId,
                orderNumber: orderNumber,
                shippingAddress: shippingAddress,
                billingAddress: request.BillingAddress,
                subTotal: 0,
                shippingFee: 30000,
                discount: 0,
                paymentMethod: request.PaymentMethod,
                note: request.Note
            );

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInfo($"Tạo đơn hàng mới: OrderId={order.Id}, UserId={request.UserId}", "OrderService");

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
                _logger.LogInfo($"Trừ kho sản phẩm Id={product.Id}, Số lượng={item.Quantity}", "ProductService");
                _unitOfWork.Products.Update(product);
            }

            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInfo($"Hoàn tất đơn hàng: OrderId={order.Id}, Tổng tiền={order.TotalAmount}", "OrderService");

            var orderDto = await _unitOfWork.Orders.GetWithDetailsAsync(order.Id, cancellationToken);
            return _mapper.Map<OrderDto>(orderDto);
        }

        private string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper()}";
        }

        private string FormatAddressToString(dynamic address)
        {
            return $"{address.Street}, {address.Ward}, {address.District}, {address.City}, {address.PostalCode}, {address.Country}";
        }
    }
}
