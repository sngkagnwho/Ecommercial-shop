using mtkpm.Application.Common.DTOs.Cart;

using mtkpm.Domain.Entities.Business;

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

        /// <summary>
        /// Xây d?ng discount chain t? danh sách mã discount
        /// </summary>
        IDiscount BuildDiscountFromCodes(IEnumerable<string> discountCodes);

        /// <summary>
        /// Xây d?ng discount chain t? danh sách discount trong database
        /// </summary>
        IDiscount BuildDiscountFromDiscountEntities(IEnumerable<Discount> discounts);
    }
}
