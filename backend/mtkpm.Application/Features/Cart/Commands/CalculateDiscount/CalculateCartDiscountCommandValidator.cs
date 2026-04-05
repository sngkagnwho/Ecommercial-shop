using FluentValidation;

namespace mtkpm.Application.Features.Cart.Commands.CalculateDiscount
{
    public class CalculateCartDiscountCommandValidator : AbstractValidator<CalculateCartDiscountCommand>
    {
        public CalculateCartDiscountCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("User ID must be greater than 0");

            RuleFor(x => x.DiscountCodes)
                .NotNull()
                .WithMessage("Discount codes cannot be null");

            RuleForEach(x => x.DiscountCodes)
                .NotEmpty()
                .WithMessage("Each discount code cannot be empty")
                .MaximumLength(50)
                .WithMessage("Discount code cannot exceed 50 characters");
        }
    }
}
