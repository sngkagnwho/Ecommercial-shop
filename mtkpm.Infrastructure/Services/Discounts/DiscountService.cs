using mtkpm.Application.Common.DTOs.Cart;
using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Events;

namespace mtkpm.Infrastructure.Services.Discounts
{
    /// <summary>
    /// Discount Service Implementation
    /// </summary>
    public class DiscountService : IDiscountService
    {
        private readonly ILoggerService _logger;

        public DiscountService(ILoggerService logger)
        {
            _logger = logger;
        }

        public DiscountInfo CalculateDiscountedPrice(CartDto cart, IDiscount discount)
        {
            if (cart == null)
                throw new ArgumentNullException(nameof(cart));

            if (discount == null)
                throw new ArgumentNullException(nameof(discount));

            _logger.LogInfo($"Calculating discounted price for cart (User: {cart.UserId}, Items: {cart.TotalItems})", "DiscountService");

            var originalAmount = cart.TotalAmount;
            var discountAmount = discount.GetDiscountAmount(cart);
            var finalAmount = discount.ApplyDiscount(cart);

            var info = new DiscountInfo
            {
                DiscountName = discount.DiscountName,
                Description = discount.Description,
                OriginalAmount = originalAmount,
                DiscountAmount = discountAmount,
                FinalAmount = finalAmount,
                AppliedDiscounts = new List<string> { discount.DiscountName }
            };

            _logger.LogInfo($"Discount applied: {info.DiscountName}, Savings: {info.DiscountAmount:C} ({info.SavingsPercent:F2}%)", "DiscountService");

            return info;
        }

        public IDiscount GetDefaultDiscounts()
        {
            _logger.LogInfo("Building default discounts", "DiscountService");

            // B?t ??u v?i BaseDiscount
            // Sau ?ó wrap v?i các decorators
            IDiscount discount = new BaseDiscount();

            // Ví d?: T? ??ng áp d?ng free shipping cho ??n > 500k
            discount = new FreeShippingDiscountDecorator(discount, shippingCost: 50000, minItemCount: 0);

            return discount;
        }

        public IDiscount BuildDiscount(params IDiscount[] discounts)
        {
            if (discounts == null || discounts.Length == 0)
                return new BaseDiscount();

            _logger.LogInfo($"Building discount chain with {discounts.Length} discounts", "DiscountService");

            // B?t ??u t? discount ??u tiên
            IDiscount result = discounts[0];

            // Wrap l?n l??t v?i các discount khác
            for (int i = 1; i < discounts.Length; i++)
            {
                if (discounts[i] is DiscountDecorator decorator)
                {
                    result = (IDiscount)Activator.CreateInstance(discounts[i].GetType(), result)
                        ?? throw new InvalidOperationException($"Cannot create instance of {discounts[i].GetType().Name}");
                }
                else
                {
                    result = discounts[i];
                }
            }

            return result;
        }
    }
}
