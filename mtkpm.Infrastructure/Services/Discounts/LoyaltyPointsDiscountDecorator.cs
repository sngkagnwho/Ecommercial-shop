using mtkpm.Application.Common.DTOs.Cart;
using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Events;

namespace mtkpm.Infrastructure.Services.Discounts
{
    /// <summary>
    /// Loyalty Points Discount Decorator - Discount by member points
    /// </summary>
    public class LoyaltyPointsDiscountDecorator : DiscountDecorator
    {
        private readonly int _loyaltyPoints;
        private readonly decimal _pointsValue; // Each point equals how much money

        /// <summary>
        /// Create loyalty points discount
        /// </summary>
        /// <param name="innerDiscount">Inner discount to wrap</param>
        /// <param name="loyaltyPoints">Member loyalty points</param>
        /// <param name="pointsValue">Value per point (default 1000)</param>
        public LoyaltyPointsDiscountDecorator(IDiscount innerDiscount, int loyaltyPoints, decimal pointsValue = 1000)
            : base(innerDiscount)
        {
            if (loyaltyPoints < 0)
                throw new ArgumentException("Loyalty points cannot be negative");

            _loyaltyPoints = loyaltyPoints;
            _pointsValue = pointsValue;
        }

        public override string DiscountName => $"{_loyaltyPoints} Points";
        public override string Description => $"S? d?ng {_loyaltyPoints} ?i?m thành viên (ti?t ki?m {GetTotalPointsValue():C})";

        protected override decimal ApplyCurrentDiscount(CartDto cart)
        {
            if (!IsApplicable(cart))
                return cart.TotalAmount;

            var discountValue = GetTotalPointsValue();
            var finalAmount = cart.TotalAmount - discountValue;
            return finalAmount > 0 ? finalAmount : 0;
        }

        public override bool IsApplicable(CartDto cart)
        {
            // Luôn có th? s? d?ng ?i?m (n?u có)
            return _loyaltyPoints > 0;
        }

        public override decimal GetDiscountAmount(CartDto cart)
        {
            return GetTotalPointsValue();
        }

        private decimal GetTotalPointsValue()
        {
            return _loyaltyPoints * _pointsValue;
        }
    }
}
