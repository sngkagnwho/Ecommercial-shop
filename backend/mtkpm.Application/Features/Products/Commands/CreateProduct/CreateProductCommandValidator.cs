using FluentValidation;

namespace mtkpm.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên sản phẩm là bắt buộc")
                .MaximumLength(200).WithMessage("Tên sản phẩm không được vượt quá 200 ký tự");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Mô tả sản phẩm là bắt buộc")
                .MaximumLength(1000).WithMessage("Mô tả không được vượt quá 1000 ký tự");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Giá phải lớn hơn hoặc bằng 0");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Số lượng tồn kho phải lớn hơn hoặc bằng 0");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Danh mục là bắt buộc");

            RuleFor(x => x.ImageUrl)
                .Must(BeAValidUrl).When(x => !string.IsNullOrEmpty(x.ImageUrl))
                .WithMessage("URL hình ảnh không hợp lệ");
        }

        private bool BeAValidUrl(string? url)
        {
            if (string.IsNullOrEmpty(url))
                return true;

            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
