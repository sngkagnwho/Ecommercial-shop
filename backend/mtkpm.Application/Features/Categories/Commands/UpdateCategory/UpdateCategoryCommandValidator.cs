using FluentValidation;

namespace mtkpm.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("ID danh m?c không h?p l?");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên danh m?c là b?t bu?c")
                .MaximumLength(100).WithMessage("Tên danh m?c không ???c v??t quá 100 ký t?");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Mô t? danh m?c là b?t bu?c")
                .MaximumLength(500).WithMessage("Mô t? không ???c v??t quá 500 ký t?");
        }
    }
}
