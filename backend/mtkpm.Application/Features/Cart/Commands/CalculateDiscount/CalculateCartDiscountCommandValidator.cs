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
                .Matches(@"^(percentage_\d+|fixed_\d+|free_shipping|loyalty_points_\d+|bundle_\d+_\d+)$")
                .WithMessage("Invalid discount code format");
        }
    }
}
