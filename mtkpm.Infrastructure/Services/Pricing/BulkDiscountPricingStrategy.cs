using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Events;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Infrastructure.Services.Pricing
{
    /// <summary>
    /// Bulk Discount Pricing Strategy - Gi?m giá khi mua nhi?u
    /// </summary>
    public class BulkDiscountPricingStrategy : IPricingStrategy
    {
        public string StrategyName => "Bulk Discount Pricing";
        public string Description => "Gi?m giá d?a vào s? l??ng mua";

        private readonly decimal _threshold;
        private readonly decimal _discountPercent;

        /// <summary>
        /// Mua t? X s?n ph?m tr? lên s? ???c gi?m Y%
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

            // N?u mua >= threshold s?n ph?m, áp d?ng discount
            if (quantity >= _threshold)
            {
                var discountAmount = basePrice * (_discountPercent / 100);
                return basePrice - discountAmount;
            }

            return basePrice;
        }
    }
}
