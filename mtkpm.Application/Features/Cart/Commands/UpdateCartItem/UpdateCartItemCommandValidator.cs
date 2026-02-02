using FluentValidation;

namespace mtkpm.Application.Features.Cart.Commands.UpdateCartItem
{
    public class UpdateCartItemCommandValidator : AbstractValidator<UpdateCartItemCommand>
    {
        public UpdateCartItemCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID không h?p l?");

            RuleFor(x => x.CartItemId)
                .GreaterThan(0).WithMessage("Cart Item ID không h?p l?");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("S? l??ng ph?i l?n h?n 0");
        }
    }
}
