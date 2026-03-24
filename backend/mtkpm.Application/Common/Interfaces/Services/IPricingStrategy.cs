using mtkpm.Domain.Entities.Business;

namespace mtkpm.Application.Common.Interfaces.Services
{
    /// <summary>
    /// Strategy Interface for Pricing Calculation
    /// S? d?ng Strategy Design Pattern ?? tính giá s?n ph?m
    /// </summary>
    public interface IPricingStrategy
    {
        /// <summary>
        /// Tính giá cu?i cùng cho s?n ph?m
        /// </summary>
        /// <param name="product">S?n ph?m</param>
        /// <param name="quantity">S? l??ng mua</param>
        /// <param name="context">Ng? c?nh (user, ngày, etc)</param>
        /// <returns>Giá cu?i cùng sau khi áp d?ng strategy</returns>
        decimal CalculatePrice(Product product, int quantity, PricingContext context);

        /// <summary>
        /// Tên strategy ?? hi?n th?
        /// </summary>
        string StrategyName { get; }

        /// <summary>
        /// Mô t? strategy
        /// </summary>
        string Description { get; }
    }
}
