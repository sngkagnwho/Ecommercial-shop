using mtkpm.Application.Common.DTOs.Cart;
using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Events;

namespace mtkpm.Infrastructure.Services.Discounts
{
    /// <summary>
    /// Fixed Amount Discount Decorator - Gi?m s? ti?n c? ??nh
    /// </summary>
    public class FixedAmountDiscountDecorator : DiscountDecorator
    {
        private readonly decimal _discountAmount;
        private readonly decimal _minOrderAmount;

        /// <summary>
        /// T?o fixed amount discount
        /// </summary>
        /// <param name="innerDiscount">Discount bên trong ?? wrap</param>
        /// <param name="discountAmount">S? ti?n gi?m c? ??nh</param>
        /// <param name="minOrderAmount">Giá t?i thi?u ??n hàng</param>
        public FixedAmountDiscountDecorator(IDiscount innerDiscount, decimal discountAmount, decimal minOrderAmount)
            : base(innerDiscount)
        {
            if (discountAmount <= 0)
                throw new ArgumentException("Discount amount must be greater than 0");

            _discountAmount = discountAmount;
            _minOrderAmount = minOrderAmount;
        }

        public override string DiscountName => $"{_discountAmount:C} Off";
        public override string Description => $"Gi?m {_discountAmount:C} cho ??n hàng t? {_minOrderAmount:C}";

        protected override decimal ApplyCurrentDiscount(CartDto cart)
        {
            if (!IsApplicable(cart))
                return cart.TotalAmount;

            var finalAmount = cart.TotalAmount - _discountAmount;
            return finalAmount > 0 ? finalAmount : 0;
        }

        public override bool IsApplicable(CartDto cart)
        {
            return cart.TotalAmount >= _minOrderAmount;
        }

        public override decimal GetDiscountAmount(CartDto cart)
        {
            if (!IsApplicable(cart))
                return 0;

            return _discountAmount;
        }
    }
}
