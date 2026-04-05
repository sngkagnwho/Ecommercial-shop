using FluentValidation;
using mtkpm.Application.Features.Discount.Validators;

namespace mtkpm.Application.Features.Discounts.Commands.UpdateDiscount
{
    public class UpdateDiscountCommandValidator : AbstractValidator<UpdateDiscountCommand>
    {
        public UpdateDiscountCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id ph?i l?n h?n 0");

            RuleFor(x => x.Dto)
                .NotNull().WithMessage("D? li?u chi?t kh?u là b?t bu?c")
                .SetValidator(new UpdateDiscountDtoValidator());
        }
    }
}
