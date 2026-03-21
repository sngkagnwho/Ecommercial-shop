using mtkpm.Application.Common.DTOs.Cart;
using mtkpm.Application.Common.Interfaces.Services;

namespace mtkpm.Infrastructure.Services.Discounts
{
    /// <summary>
    /// Base Discount - Component cho Decorator Pattern
    /// ?ây là discount c? b?n không có chi?t kh?u
    /// </summary>
    public class BaseDiscount : IDiscount
    {
        public virtual string DiscountName => "No Discount";
        public virtual string Description => "Giá bán th??ng không có chi?t kh?u";

        public virtual decimal ApplyDiscount(CartDto cart)
        {
            if (cart == null)
                throw new ArgumentNullException(nameof(cart));

            // Không áp d?ng discount nào
            return cart.TotalAmount;
        }

        public virtual bool IsApplicable(CartDto cart)
        {
            return true; // Luôn có th? áp d?ng
        }

        public virtual decimal GetDiscountAmount(CartDto cart)
        {
            return 0; // Không có chi?t kh?u
        }
    }
}
