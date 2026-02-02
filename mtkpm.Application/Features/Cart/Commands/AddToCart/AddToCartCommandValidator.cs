using FluentValidation;

namespace mtkpm.Application.Features.Cart.Commands.AddToCart
{
    public class AddToCartCommandValidator : AbstractValidator<AddToCartCommand>
    {
        public AddToCartCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID không h?p l?");

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Product ID không h?p l?");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("S? l??ng ph?i l?n h?n 0");
        }
    }
}
