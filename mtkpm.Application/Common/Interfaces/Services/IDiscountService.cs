using mtkpm.Application.Common.DTOs.Cart;

namespace mtkpm.Application.Common.Interfaces.Services
{
    /// <summary>
    /// Discount Service Interface - Orchestrate discount decorators
    /// </summary>
    public interface IDiscountService
    {
        /// <summary>
        /// Tính giá gi? hàng sau khi áp d?ng discounts
        /// </summary>
        DiscountInfo CalculateDiscountedPrice(CartDto cart, IDiscount discount);

        /// <summary>
        /// L?y các decorator m?c ??nh
        /// </summary>
        IDiscount GetDefaultDiscounts();

        /// <summary>
        /// Xây d?ng discount t? danh sách
        /// </summary>
        IDiscount BuildDiscount(params IDiscount[] discounts);
    }
}
