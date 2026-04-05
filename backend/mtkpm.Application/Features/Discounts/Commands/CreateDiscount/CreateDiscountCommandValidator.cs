using FluentValidation;
using mtkpm.Application.Features.Discount.Validators;

namespace mtkpm.Application.Features.Discounts.Commands.CreateDiscount
{
    public class CreateDiscountCommandValidator : AbstractValidator<CreateDiscountCommand>
    {
        public CreateDiscountCommandValidator()
        {
            RuleFor(x => x.Dto)
                .NotNull().WithMessage("D? li?u chi?t kh?u là b?t bu?c")
                .SetValidator(new CreateDiscountDtoValidator());
        }
    }
}
