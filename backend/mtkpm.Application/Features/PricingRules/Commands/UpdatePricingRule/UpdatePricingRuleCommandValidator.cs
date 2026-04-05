using FluentValidation;

namespace mtkpm.Application.Features.PricingRules.Commands.UpdatePricingRule
{
    public class UpdatePricingRuleCommandValidator : AbstractValidator<UpdatePricingRuleCommand>
    {
        public UpdatePricingRuleCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id ph?i l?n h?n 0");

            RuleFor(x => x.Dto.Name)
                .NotEmpty().WithMessage("Tên quy t?c ??nh giá là b?t bu?c")
                .MaximumLength(100).WithMessage("Tên quy t?c ??nh giá không v??t quá 100 ký t?");

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
