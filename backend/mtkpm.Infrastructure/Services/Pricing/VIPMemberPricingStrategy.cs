using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Events;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Infrastructure.Services.Pricing
{
    /// <summary>
    /// VIP Member Pricing Strategy - Special price for VIP members
    /// Tier: Bronze, Silver, Gold, Platinum
    /// </summary>
    public class VIPMemberPricingStrategy : IPricingStrategy
    {
        public string StrategyName => "VIP Member Pricing";
        public string Description => "Special pricing for VIP members";

        private readonly Dictionary<string, decimal> _tierDiscounts = new()
        {
            { "Bronze", 0.05m },      // 5% discount
            { "Silver", 0.10m },      // 10% discount
            { "Gold", 0.15m },        // 15% discount
            { "Platinum", 0.25m }     // 25% discount
        };

        public decimal CalculatePrice(Product product, int quantity, PricingContext context)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var basePrice = product.Price * quantity;

            // If VIP member, apply discount by tier
            if (!string.IsNullOrEmpty(context.UserTier) && 
                _tierDiscounts.TryGetValue(context.UserTier, out var discountPercent))
            {
                var discountAmount = basePrice * discountPercent;
                return basePrice - discountAmount;
            }

            // If not VIP, return regular price
            return basePrice;
        }
    }
}
