using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Events;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Infrastructure.Services.Pricing
{
    /// <summary>
    /// Seasonal Pricing Strategy - Price by season/occasion (Black Friday, Tet, etc)
    /// </summary>
    public class SeasonalPricingStrategy : IPricingStrategy
    {
        public string StrategyName => "Seasonal Pricing";
        public string Description => "Price by season/special occasions";

        private readonly Dictionary<(int Month, int Day), decimal> _seasonalDiscounts;

        public SeasonalPricingStrategy()
        {
            // Setup seasonal sales throughout the year
            _seasonalDiscounts = new Dictionary<(int Month, int Day), decimal>
            {
                // Tet Lunar New Year (approx Feb)
                { (2, 1), 0.15m },  // 15% discount
                
                // Black Friday (11/27)
                { (11, 27), 0.25m }, // 25% discount
                
                // Cyber Monday (11/30)
                { (11, 30), 0.20m }, // 20% discount
                
                // Christmas (12/25)
                { (12, 25), 0.20m }, // 20% discount
                
                // Flash Sale Summer (6/1-6/7)
                { (6, 1), 0.10m },   // 10% discount
                
                // Back to School (8/15-8/31)
                { (8, 15), 0.12m }   // 12% discount
            };
        }

        public decimal CalculatePrice(Product product, int quantity, PricingContext context)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var basePrice = product.Price * quantity;
            var currentDate = context.CurrentDate;
            var seasonKey = (currentDate.Month, currentDate.Day);

            // Check if today has a sale
            if (_seasonalDiscounts.TryGetValue(seasonKey, out var discountPercent))
            {
                var discountAmount = basePrice * discountPercent;
                return basePrice - discountAmount;
            }

            // If no sale, return regular price
            return basePrice;
        }
    }
}
