using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Infrastructure.Services.Pricing
{
    /// <summary>
    /// Regular Pricing Strategy - Giá th??ng không có gi?m giá
    /// </summary>
    public class RegularPricingStrategy : IPricingStrategy
    {
        public string StrategyName => "Regular Pricing";
        public string Description => "Giá bán th??ng không có chi?t kh?u";

        public decimal CalculatePrice(Product product, int quantity, PricingContext context)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            // Giá th??ng = giá s?n ph?m * s? l??ng
            return product.Price * quantity;
        }
    }
}
