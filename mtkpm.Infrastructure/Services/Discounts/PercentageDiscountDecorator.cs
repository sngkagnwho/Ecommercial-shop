using mtkpm.Application.Common.DTOs.Cart;
using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Events;

namespace mtkpm.Infrastructure.Services.Discounts
{
    /// <summary>
    /// Percentage Discount Decorator - Discount by percentage
    /// </summary>
    public class PercentageDiscountDecorator : DiscountDecorator
    {
        private readonly decimal _discountPercent;
        private readonly decimal _minAmount;

        /// <summary>
        /// Create percentage discount
        /// </summary>
        /// <param name="innerDiscount">Inner discount to wrap</param>
        /// <param name="discountPercent">Discount percentage (0-100)</param>
        /// <param name="minAmount">Minimum amount to apply (default 0)</param>
        public PercentageDiscountDecorator(IDiscount innerDiscount, decimal discountPercent, decimal minAmount = 0)
            : base(innerDiscount)
        {
            if (discountPercent <= 0 || discountPercent > 100)
                throw new ArgumentException("Discount percent must be between 0 and 100");

            _discountPercent = discountPercent;
            _minAmount = minAmount;
        }

        public override string DiscountName => $"{_discountPercent}% Off";
        public override string Description => $"Gi?m {_discountPercent}% cho ??n hàng t? {_minAmount:C}";

        protected override decimal ApplyCurrentDiscount(CartDto cart)
        {
            if (!IsApplicable(cart))
                return cart.TotalAmount;

            var discountAmount = cart.TotalAmount * (_discountPercent / 100);
            return cart.TotalAmount - discountAmount;
        }

        public override bool IsApplicable(CartDto cart)
        {
            return cart.TotalAmount >= _minAmount;
        }

        public override decimal GetDiscountAmount(CartDto cart)
        {
            if (!IsApplicable(cart))
                return 0;

            return cart.TotalAmount * (_discountPercent / 100);
        }
    }
}
