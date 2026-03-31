using FluentValidation;

namespace mtkpm.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID không hợp lệ");

            RuleFor(x => x.ShippingAddress)
                .NotEmpty().WithMessage("Địa chỉ giao hàng là bắt buộc")
                .MaximumLength(500).WithMessage("Địa chỉ giao hàng không được vượt quá 500 ký tự");

            RuleFor(x => x.BillingAddress)
                .MaximumLength(500).WithMessage("Địa chỉ thanh toản không được vượt quá 500 ký tự")
                .When(x => !string.IsNullOrEmpty(x.BillingAddress));

            RuleFor(x => x.PaymentMethod)
                .IsInEnum().WithMessage("Phương thức thanh toản không hợp lệ");

            RuleFor(x => x.Note)
                .MaximumLength(500).WithMessage("Ghi chú không được vượt quá 500 ký tự")
                .When(x => !string.IsNullOrEmpty(x.Note));

            RuleFor(x => x.OrderItems)
                .NotEmpty().WithMessage("Đơn hàng phải có ít nhất 1 sản phẩm")
                .Must(items => items.Count > 0).WithMessage("Đơn hàng phải có ít nhất 1 sản phẩm");

            RuleForEach(x => x.OrderItems).ChildRules(item =>
            {
                item.RuleFor(x => x.ProductId)
                    .GreaterThan(0).WithMessage("ID sản phẩm không hợp lệ");

                item.RuleFor(x => x.Quantity)
                    .GreaterThan(0).WithMessage("Số lượng phải lớn hơn 0");
            });
        }
    }
}
