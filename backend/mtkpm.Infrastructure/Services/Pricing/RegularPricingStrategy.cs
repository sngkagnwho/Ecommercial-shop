using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Events;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Infrastructure.Services.Pricing
{
    /// <summary>
    /// Regular Pricing Strategy - Giá thường không có giảm giá
    /// </summary>
    public class RegularPricingStrategy : IPricingStrategy
    {
        public string StrategyName => "Regular Pricing";
        public string Description => "Giá bán thường không có chiết khấu";

        public decimal CalculatePrice(Product product, int quantity, PricingContext context)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            // Giá thường = giá sản phẩm * số lượng
            return product.Price * quantity;
        }
    }
}
