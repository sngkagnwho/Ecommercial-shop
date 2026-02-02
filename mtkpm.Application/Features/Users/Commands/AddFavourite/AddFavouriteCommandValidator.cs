using FluentValidation;

namespace mtkpm.Application.Features.Users.Commands.AddFavourite
{
    public class AddFavouriteCommandValidator : AbstractValidator<AddFavouriteCommand>
    {
        public AddFavouriteCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID không h?p l?");

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Product ID không h?p l?");
        }
    }
}
