using FluentValidation;

namespace mtkpm.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên s?n ph?m là b?t bu?c")
                .MaximumLength(200).WithMessage("Tên s?n ph?m không ???c v??t quá 200 ký t?");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Mô t? s?n ph?m là b?t bu?c")
                .MaximumLength(1000).WithMessage("Mô t? không ???c v??t quá 1000 ký t?");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Giá ph?i l?n h?n ho?c b?ng 0");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("S? l??ng t?n kho ph?i l?n h?n ho?c b?ng 0");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Danh m?c là b?t bu?c");

            RuleFor(x => x.ImageUrl)
                .Must(BeAValidUrl).When(x => !string.IsNullOrEmpty(x.ImageUrl))
                .WithMessage("URL hình ?nh không h?p l?");
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
