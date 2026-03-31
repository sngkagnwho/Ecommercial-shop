using FluentValidation;

namespace mtkpm.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên danh mục là bắt buộc")
                .MaximumLength(100).WithMessage("Tên danh mục không được vượt quá 100 ký tự");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Mô tả danh mục là bắt buộc")
                .MaximumLength(500).WithMessage("Mô tả không được vượt quá 500 ký tự");
        }
    }
}
