using mtkpm.Application.Common.DTOs.Cart;
using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Events;

namespace mtkpm.Infrastructure.Services.Discounts
{
    /// <summary>
    /// Discount Decorator - Base class for all decorators
    /// Allows stacking discounts (applying multiple discounts sequentially)
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
        /// Apply this decorator then call the inner decorator
        /// This allows stacking (e.g.: 10% discount + Free shipping)
        /// </summary>
        public virtual decimal ApplyDiscount(CartDto cart)
        {
            if (cart == null)
                throw new ArgumentNullException(nameof(cart));

            // Apply inner discount first
            var priceAfterInnerDiscount = _innerDiscount.ApplyDiscount(cart);
            
            // Create a temporary cart with the discounted price from the inner discount
            var tempCart = new CartDto
            {
                UserId = cart.UserId,
                Items = cart.Items,
                TotalItems = cart.TotalItems,
                TotalAmount = priceAfterInnerDiscount
            };

            // Apply the current discount
            return ApplyCurrentDiscount(tempCart);
        }

        /// <summary>
        /// Apply the discount of the current decorator
        /// Subclass override this method
        /// </summary>
        protected abstract decimal ApplyCurrentDiscount(CartDto cart);

        public abstract bool IsApplicable(CartDto cart);

        public abstract decimal GetDiscountAmount(CartDto cart);
    }
}
