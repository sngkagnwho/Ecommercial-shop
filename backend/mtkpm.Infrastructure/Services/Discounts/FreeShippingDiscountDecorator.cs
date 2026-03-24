using mtkpm.Application.Common.DTOs.Cart;
using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Events;

namespace mtkpm.Infrastructure.Services.Discounts
{
    /// <summary>
    /// Free Shipping Discount Decorator - Free shipping
    /// </summary>
    public class FreeShippingDiscountDecorator : DiscountDecorator
    {
        private readonly decimal _shippingCost;
        private readonly int _minItemCount;

        /// <summary>
        /// Create free shipping discount
        /// </summary>
        /// <param name="innerDiscount">Inner discount to wrap</param>
        /// <param name="shippingCost">Standard shipping cost</param>
        /// <param name="minItemCount">Minimum number of items (default 0)</param>
        public FreeShippingDiscountDecorator(IDiscount innerDiscount, decimal shippingCost, int minItemCount = 0)
            : base(innerDiscount)
        {
            if (shippingCost < 0)
                throw new ArgumentException("Shipping cost cannot be negative");

            _shippingCost = shippingCost;
            _minItemCount = minItemCount;
        }

        public override string DiscountName => "Free Shipping";
        public override string Description => $"Mi?n phí v?n chuy?n (ti?t ki?m {_shippingCost:C}) cho {_minItemCount}+ s?n ph?m";

        protected override decimal ApplyCurrentDiscount(CartDto cart)
        {
            if (!IsApplicable(cart))
                return cart.TotalAmount;

            // Gi?m chi phí v?n chuy?n
            return cart.TotalAmount - _shippingCost;
        }

        public override bool IsApplicable(CartDto cart)
        {
            return cart.TotalItems >= _minItemCount;
        }

        public override decimal GetDiscountAmount(CartDto cart)
        {
            if (!IsApplicable(cart))
                return 0;

            return _shippingCost;
        }
    }
}
