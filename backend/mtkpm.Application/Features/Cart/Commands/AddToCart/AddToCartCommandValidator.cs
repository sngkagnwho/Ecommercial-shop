using FluentValidation;

namespace mtkpm.Application.Features.Cart.Commands.AddToCart
{
    public class AddToCartCommandValidator : AbstractValidator<AddToCartCommand>
    {
        public AddToCartCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID không hợp lệ");

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Product ID không hợp lệ");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Số lượng phải lớn hơn 0");
        }
    }
}
