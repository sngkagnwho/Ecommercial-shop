using FluentValidation;

namespace mtkpm.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên danh m?c là b?t bu?c")
                .MaximumLength(100).WithMessage("Tên danh m?c không ???c v??t quá 100 ký t?");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Mô t? danh m?c là b?t bu?c")
                .MaximumLength(500).WithMessage("Mô t? không ???c v??t quá 500 ký t?");
        }
    }
}
