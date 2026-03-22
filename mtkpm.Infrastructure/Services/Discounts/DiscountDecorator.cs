using mtkpm.Application.Common.DTOs.Cart;
using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Events;

namespace mtkpm.Infrastructure.Services.Discounts
{
    /// <summary>
    /// Discount Decorator - Base class cho t?t c? decorators
    /// Cho phép stacking discounts (áp d?ng nhi?u discount l?n l??t)
    /// </summary>
    public abstract class DiscountDecorator : IDiscount
    {
        protected readonly IDiscount _innerDiscount;

        protected DiscountDecorator(IDiscount innerDiscount)
        {
            _innerDiscount = innerDiscount ?? throw new ArgumentNullException(nameof(innerDiscount));
        }

        public abstract string DiscountName { get; }
        public abstract string Description { get; }

        /// <summary>
        /// Áp d?ng decorator này r?i g?i decorator trong cùng
        /// ?i?u này cho phép stacking (ví d?: 10% discount + Free shipping)
        /// </summary>
        public virtual decimal ApplyDiscount(CartDto cart)
        {
            if (cart == null)
                throw new ArgumentNullException(nameof(cart));

            // Áp d?ng discount bên trong tr??c
            var priceAfterInnerDiscount = _innerDiscount.ApplyDiscount(cart);
            
            // T?o cart t?m th?i v?i giá ?ã ???c discount t? bên trong
            var tempCart = new CartDto
            {
                UserId = cart.UserId,
                Items = cart.Items,
                TotalItems = cart.TotalItems,
                TotalAmount = priceAfterInnerDiscount
            };

            // Áp d?ng discount hi?n t?i
            return ApplyCurrentDiscount(tempCart);
        }

        /// <summary>
        /// Áp d?ng discount c?a decorator hi?n t?i
        /// Subclass override method này
        /// </summary>
        protected abstract decimal ApplyCurrentDiscount(CartDto cart);

        public abstract bool IsApplicable(CartDto cart);

        public abstract decimal GetDiscountAmount(CartDto cart);
    }
}
