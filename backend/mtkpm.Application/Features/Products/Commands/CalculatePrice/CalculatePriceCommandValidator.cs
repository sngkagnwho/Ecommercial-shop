using FluentValidation;

namespace mtkpm.Application.Features.Products.Commands.CalculatePrice
{
    public class CalculatePriceCommandValidator : AbstractValidator<CalculatePriceCommand>
    {
        public CalculatePriceCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("Product ID must be greater than 0");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0")
                .LessThanOrEqualTo(10000)
                .WithMessage("Quantity cannot exceed 10,000");

            RuleFor(x => x.PricingStrategy)
                .Must(strategy => strategy == null || IsValidStrategy(strategy))
                .WithMessage("Invalid pricing strategy. Valid options: 'regular', 'bulk', 'seasonal', 'vip'")
                .When(x => !string.IsNullOrEmpty(x.PricingStrategy));

            RuleFor(x => x.UserTier)
                .Must(tier => tier == null || IsValidTier(tier))
                .WithMessage("Invalid user tier. Valid options: 'Bronze', 'Silver', 'Gold', 'Platinum'")
                .When(x => !string.IsNullOrEmpty(x.UserTier));
        }

        private static bool IsValidStrategy(string strategy)
        {
            var validStrategies = new[] { "regular", "bulk", "seasonal", "vip" };
            return validStrategies.Contains(strategy.ToLower());
        }

        private static bool IsValidTier(string tier)
        {
            var validTiers = new[] { "Bronze", "Silver", "Gold", "Platinum" };
            return validTiers.Contains(tier, StringComparer.OrdinalIgnoreCase);
        }
    }
}
