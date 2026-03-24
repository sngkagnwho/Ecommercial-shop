using FluentValidation;

namespace mtkpm.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID không h?p l?");

            RuleFor(x => x.ShippingAddress)
                .NotEmpty().WithMessage("??a ch? giao hàng là b?t bu?c")
                .MaximumLength(500).WithMessage("??a ch? giao hàng không ???c v??t quá 500 ký t?");

            RuleFor(x => x.BillingAddress)
                .MaximumLength(500).WithMessage("??a ch? thanh toán không ???c v??t quá 500 ký t?")
                .When(x => !string.IsNullOrEmpty(x.BillingAddress));

            RuleFor(x => x.PaymentMethod)
                .IsInEnum().WithMessage("Ph??ng th?c thanh toán không h?p l?");

            RuleFor(x => x.Note)
                .MaximumLength(500).WithMessage("Ghi chú không ???c v??t quá 500 ký t?")
                .When(x => !string.IsNullOrEmpty(x.Note));

            RuleFor(x => x.OrderItems)
                .NotEmpty().WithMessage("??n hàng ph?i có ít nh?t 1 s?n ph?m")
                .Must(items => items.Count > 0).WithMessage("??n hàng ph?i có ít nh?t 1 s?n ph?m");

            RuleForEach(x => x.OrderItems).ChildRules(item =>
            {
                item.RuleFor(x => x.ProductId)
                    .GreaterThan(0).WithMessage("ID s?n ph?m không h?p l?");

                item.RuleFor(x => x.Quantity)
                    .GreaterThan(0).WithMessage("S? l??ng ph?i l?n h?n 0");
            });
        }
    }
}
