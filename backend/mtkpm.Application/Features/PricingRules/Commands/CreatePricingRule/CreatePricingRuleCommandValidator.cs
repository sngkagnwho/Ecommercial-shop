using FluentValidation;

namespace mtkpm.Application.Features.PricingRules.Commands.CreatePricingRule
{
    public class CreatePricingRuleCommandValidator : AbstractValidator<CreatePricingRuleCommand>
    {
        public CreatePricingRuleCommandValidator()
        {
            RuleFor(x => x.Dto.Name)
                .NotEmpty().WithMessage("Tên quy t?c ??nh giá là b?t bu?c")
                .MaximumLength(100).WithMessage("Tên quy t?c ??nh giá không v??t quá 100 ký t?");

            RuleFor(x => x.Dto.RuleType)
                .NotEmpty().WithMessage("Lo?i quy t?c là b?t bu?c")
                .MaximumLength(50).WithMessage("Lo?i quy t?c không v??t quá 50 ký t?");

            RuleFor(x => x.Dto.RuleCondition)
                .NotEmpty().WithMessage("?i?u ki?n áp d?ng là b?t bu?c")
                .MaximumLength(1000).WithMessage("?i?u ki?n áp d?ng không v??t quá 1000 ký t?");

            RuleFor(x => x.Dto.EndDate)
                .GreaterThan(x => x.Dto.StartDate).WithMessage("Ngày k?t thúc ph?i l?n h?n ngày b?t ??u");

            RuleFor(x => x.Dto.Priority)
                .GreaterThanOrEqualTo(0).WithMessage("?? ?u tiên ph?i >= 0");
        }
    }
}
