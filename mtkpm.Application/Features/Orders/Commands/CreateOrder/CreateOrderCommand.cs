using MediatR;
using mtkpm.Application.Common.DTOs.Order;
using mtkpm.Domain.Enums.Business;

namespace mtkpm.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<OrderDto>
    {
        public int UserId { get; set; }
        public string ShippingAddress { get; set; }
        public string? BillingAddress { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string? Note { get; set; }
        public List<CreateOrderItemDto> OrderItems { get; set; } = new();
    }

    public class CreateOrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
