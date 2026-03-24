using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Events;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Infrastructure.Services.Pricing
{
    /// <summary>
    /// Bulk Discount Pricing Strategy - Discount when buying many
    /// </summary>
    public class BulkDiscountPricingStrategy : IPricingStrategy
    {
        public string StrategyName => "Bulk Discount Pricing";
        public string Description => "Discount based on quantity purchased";

        private readonly decimal _threshold;
        private readonly decimal _discountPercent;

        /// <summary>
        /// Buy from X products and above will get Y% discount
        /// </summary>
        public BulkDiscountPricingStrategy(int threshold = 10, decimal discountPercent = 10m)
        {
            _threshold = threshold;
            _discountPercent = discountPercent;
        }

        public decimal CalculatePrice(Product product, int quantity, PricingContext context)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var basePrice = product.Price * quantity;

            // If buy >= threshold products, apply discount
            if (quantity >= _threshold)
            {
                var discountAmount = basePrice * (_discountPercent / 100);
                return basePrice - discountAmount;
            }

            return basePrice;
        }
    }
}
