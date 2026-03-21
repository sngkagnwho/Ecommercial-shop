using mtkpm.Application.Common.DTOs.Cart;

namespace mtkpm.Application.Common.Interfaces.Services
{
    /// <summary>
    /// Discount Interface - Component for Decorator Pattern
    /// ??nh ngh?a c?u trúc cho discount và decorators
    /// </summary>
    public interface IDiscount
    {
        /// <summary>
        /// Tính toán giá sau khi áp d?ng discount
        /// </summary>
        decimal ApplyDiscount(CartDto cart);

        /// <summary>
        /// Tên discount ?? hi?n th?
        /// </summary>
        string DiscountName { get; }

        /// <summary>
        /// Mô t? discount
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Ki?m tra xem discount có áp d?ng ???c không
        /// </summary>
        bool IsApplicable(CartDto cart);

        /// <summary>
        /// L?i ích (ti?n ti?t ki?m) t? discount này
        /// </summary>
        decimal GetDiscountAmount(CartDto cart);
    }

    /// <summary>
    /// Discount Info - Thông tin chi ti?t v? discount
    /// </summary>
    public class DiscountInfo
    {
        public string DiscountName { get; set; }
        public string Description { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public decimal SavingsPercent => OriginalAmount > 0 ? (DiscountAmount / OriginalAmount) * 100 : 0;
        public List<string> AppliedDiscounts { get; set; } = new();
    }
}
