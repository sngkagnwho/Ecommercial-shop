using FluentValidation;

namespace mtkpm.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("ID danh mục không hợp lệ");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên danh mục là bắt buộc")
                .MaximumLength(100).WithMessage("Tên danh mục không được vượt quá 100 ký tự");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Mô tả danh mục là bắt buộc")
                .MaximumLength(500).WithMessage("Mô tả không được vượt quá 500 ký tự");
        }
    }
}
