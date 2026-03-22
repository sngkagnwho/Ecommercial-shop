using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Events;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Infrastructure.Services.Pricing
{
    /// <summary>
    /// VIP Member Pricing Strategy - Giá cho thành viên VIP
    /// Phân c?p: Bronze, Silver, Gold, Platinum
    /// </summary>
    public class VIPMemberPricingStrategy : IPricingStrategy
    {
        public string StrategyName => "VIP Member Pricing";
        public string Description => "Giá ??c bi?t cho thành viên VIP";

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

            // N?u là VIP member, áp d?ng discount theo tier
            if (!string.IsNullOrEmpty(context.UserTier) && 
                _tierDiscounts.TryGetValue(context.UserTier, out var discountPercent))
            {
                var discountAmount = basePrice * discountPercent;
                return basePrice - discountAmount;
            }

            // N?u không ph?i VIP, tr? v? giá th??ng
            return basePrice;
        }
    }
}
