using mtkpm.Application.Common.DTOs.Cart;
using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Events;

namespace mtkpm.Infrastructure.Services.Discounts
{
    /// <summary>
    /// Base Discount - Component cho Decorator Pattern
    /// ?�y l� discount c? b?n kh�ng c� chi?t kh?u
    /// </summary>
    public class BaseDiscount : IDiscount
    {
        public virtual string DiscountName => "No Discount";
        public virtual string Description => "Giá bán thường không có chiết khấu";

        public virtual decimal ApplyDiscount(CartDto cart)
        {
            if (cart == null)
                throw new ArgumentNullException(nameof(cart));

            // Kh�ng �p d?ng discount n�o
            return cart.TotalAmount;
        }

        public virtual bool IsApplicable(CartDto cart)
        {
            return true; // Lu�n c� th? �p d?ng
        }

        public virtual decimal GetDiscountAmount(CartDto cart)
        {
            return 0; // Kh�ng c� chi?t kh?u
        }
    }
}
