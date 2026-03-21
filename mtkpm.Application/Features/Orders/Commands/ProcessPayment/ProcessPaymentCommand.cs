using MediatR;
using mtkpm.Application.Common.DTOs.Order;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Enums.Business;

namespace mtkpm.Application.Features.Orders.Commands.ProcessPayment
{
    public class ProcessPaymentCommand : IRequest<ProcessPaymentResponse>
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethodType PaymentMethod { get; set; }
    }

    public class ProcessPaymentResponse
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; }
        public string Message { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
