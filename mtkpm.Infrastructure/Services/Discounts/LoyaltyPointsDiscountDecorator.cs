using mtkpm.Application.Common.DTOs.Cart;
using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Events;

namespace mtkpm.Infrastructure.Services.Discounts
{
    /// <summary>
    /// Loyalty Points Discount Decorator - Gi?m theo ?i?m thành viên
    /// </summary>
    public class LoyaltyPointsDiscountDecorator : DiscountDecorator
    {
        private readonly int _loyaltyPoints;
        private readonly decimal _pointsValue; // M?i ?i?m b?ng bao nhiêu ti?n

        /// <summary>
        /// T?o loyalty points discount
        /// </summary>
        /// <param name="innerDiscount">Discount bên trong ?? wrap</param>
        /// <param name="loyaltyPoints">S? ?i?m thành viên</param>
        /// <param name="pointsValue">Giá tr? m?i ?i?m (m?c ??nh 1000 ? = 1 ?i?m)</param>
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
