using FluentValidation;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Enums.Business;

namespace mtkpm.Application.Features.Orders.Commands.ProcessPayment
{
    public class ProcessPaymentCommandValidator : AbstractValidator<ProcessPaymentCommand>
    {
        private readonly IPaymentFactory _paymentFactory;

        public ProcessPaymentCommandValidator(IPaymentFactory paymentFactory)
        {
            _paymentFactory = paymentFactory;

            RuleFor(x => x.OrderId)
                .GreaterThan(0)
                .WithMessage("Order ID must be greater than 0");

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than 0");

            RuleFor(x => x.PaymentMethod)
                .Must(x => _paymentFactory.IsPaymentMethodSupported(x))
                .WithMessage("Payment method is not supported");
        }
    }
}
