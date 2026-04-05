using FluentValidation;
using mtkpm.Application.Common.DTOs.Discount;

namespace mtkpm.Application.Features.Discount.Validators
{
    public class CreateDiscountDtoValidator : AbstractValidator<CreateDiscountDto>
    {
        public CreateDiscountDtoValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Mã chi?t kh?u không ???c ?? tr?ng")
                .MinimumLength(3).WithMessage("Mã ph?i ít nh?t 3 ký t?")
                .MaximumLength(50).WithMessage("Mã không ???c quá 50 ký t?")
                .Matches(@"^[A-Z0-9_]+$").WithMessage("Mã ch? ch?a ch? in hoa, s? và d?u g?ch d??i");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên chi?t kh?u không ???c ?? tr?ng")
                .MinimumLength(3).WithMessage("Tên ph?i ít nh?t 3 ký t?")
                .MaximumLength(100).WithMessage("Tên không ???c quá 100 ký t?");

            RuleFor(x => x.DiscountType)
                .NotEmpty().WithMessage("Lo?i chi?t kh?u không ???c ?? tr?ng")
                .Must(x => new[] { "Percentage", "FixedAmount", "FreeShipping" }.Contains(x))
                .WithMessage("Lo?i chi?t kh?u không h?p l?. H? tr?: Percentage, FixedAmount, FreeShipping");

            RuleFor(x => x.DiscountValue)
                .GreaterThan(0).WithMessage("Giá tr? chi?t kh?u ph?i l?n h?n 0")
                .When(x => x.DiscountType != "FreeShipping");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Ngày b?t ??u không ???c ?? tr?ng");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("Ngày k?t thúc không ???c ?? tr?ng")
                .GreaterThan(x => x.StartDate).WithMessage("Ngày k?t thúc ph?i sau ngày b?t ??u");

            RuleFor(x => x.MinimumOrderAmount)
                .GreaterThan(0).WithMessage("Giá tr? ??n hàng t?i thi?u ph?i l?n h?n 0")
                .When(x => x.MinimumOrderAmount.HasValue);

            RuleFor(x => x.MaximumDiscountAmount)
                .GreaterThan(0).WithMessage("Giá tr? chi?t kh?u t?i ?a ph?i l?n h?n 0")
                .When(x => x.MaximumDiscountAmount.HasValue);

            RuleFor(x => x.BudgetLimit)
                .GreaterThan(0).WithMessage("Ngân sách t?i ?a ph?i l?n h?n 0")
                .When(x => x.BudgetLimit.HasValue);

            RuleFor(x => x.MaxUsageCount)
                .GreaterThan(0).WithMessage("S? l?n s? d?ng ph?i l?n h?n 0")
                .When(x => x.MaxUsageCount.HasValue);

            RuleFor(x => x.MaxUsagePerUser)
                .GreaterThan(0).WithMessage("S? l?n s? d?ng m?i user ph?i l?n h?n 0")
                .When(x => x.MaxUsagePerUser.HasValue);
        }
    }

    public class UpdateDiscountDtoValidator : AbstractValidator<UpdateDiscountDto>
    {
        public UpdateDiscountDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên chi?t kh?u không ???c ?? tr?ng")
                .MinimumLength(3).WithMessage("Tên ph?i ít nh?t 3 ký t?")
                .MaximumLength(100).WithMessage("Tên không ???c quá 100 ký t?");

            RuleFor(x => x.DiscountType)
                .NotEmpty().WithMessage("Lo?i chi?t kh?u không ???c ?? tr?ng")
                .Must(x => new[] { "Percentage", "FixedAmount", "FreeShipping" }.Contains(x))
                .WithMessage("Lo?i chi?t kh?u không h?p l?");

            RuleFor(x => x.DiscountValue)
                .GreaterThan(0).WithMessage("Giá tr? chi?t kh?u ph?i l?n h?n 0")
                .When(x => x.DiscountType != "FreeShipping");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Ngày b?t ??u không ???c ?? tr?ng");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("Ngày k?t thúc không ???c ?? tr?ng")
                .GreaterThan(x => x.StartDate).WithMessage("Ngày k?t thúc ph?i sau ngày b?t ??u");
        }
    }
}
