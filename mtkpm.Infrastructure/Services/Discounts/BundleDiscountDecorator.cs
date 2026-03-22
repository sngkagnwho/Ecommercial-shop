using mtkpm.Application.Common.DTOs.Cart;
using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Events;

namespace mtkpm.Infrastructure.Services.Discounts
{
    /// <summary>
    /// Bundle Discount Decorator - Discount when buying combo
    /// </summary>
    public class BundleDiscountDecorator : DiscountDecorator
    {
        private readonly int _requiredItemCount;
        private readonly decimal _discountPercent;

        /// <summary>
        /// Create bundle discount
        /// </summary>
        /// <param name="innerDiscount">Inner discount to wrap</param>
        /// <param name="requiredItemCount">Minimum number of items to apply</param>
        /// <param name="discountPercent">Discount percentage</param>
        public BundleDiscountDecorator(IDiscount innerDiscount, int requiredItemCount, decimal discountPercent)
            : base(innerDiscount)
        {
            if (requiredItemCount <= 0)
                throw new ArgumentException("Required item count must be greater than 0");

            if (discountPercent <= 0 || discountPercent > 100)
                throw new ArgumentException("Discount percent must be between 0 and 100");

            _requiredItemCount = requiredItemCount;
            _discountPercent = discountPercent;
        }

        public override string DiscountName => $"Bundle Discount ({_requiredItemCount}+ items)";
        public override string Description => $"Mua {_requiredItemCount}+ s?n ph?m ???c gi?m {_discountPercent}%";

        protected override decimal ApplyCurrentDiscount(CartDto cart)
        {
            if (!IsApplicable(cart))
                return cart.TotalAmount;

            var discountAmount = cart.TotalAmount * (_discountPercent / 100);
            return cart.TotalAmount - discountAmount;
        }

        public override bool IsApplicable(CartDto cart)
        {
            return cart.TotalItems >= _requiredItemCount;
        }

        public override decimal GetDiscountAmount(CartDto cart)
        {
            if (!IsApplicable(cart))
                return 0;

            return cart.TotalAmount * (_discountPercent / 100);
        }
    }
}
