using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Application.Common.Interfaces.Services
{
    /// <summary>
    /// Pricing Service Interface - Orchestrate pricing strategies
    /// Implementation t? Infrastructure Layer
    /// </summary>
    public interface IPricingService
    {
        /// <summary>
        /// Tính giá s?n ph?m dùng strategy ???c ch? ??nh
        /// </summary>
        decimal CalculatePrice(Product product, int quantity, IPricingStrategy strategy, PricingContext context);

        /// <summary>
        /// Tính giá s?n ph?m dùng strategy t?t nh?t cho ng??i dùng
        /// </summary>
        decimal CalculateBestPrice(Product product, int quantity, PricingContext context);

        /// <summary>
        /// L?y danh sách t?t c? strategies có s?n
        /// </summary>
        IEnumerable<IPricingStrategy> GetAvailableStrategies();

        /// <summary>
        /// L?y strategy theo tên
        /// </summary>
        IPricingStrategy? GetStrategyByName(string strategyName);
    }
}
