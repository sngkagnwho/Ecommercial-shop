using mtkpm.Application.Common.DTOs.Cart;
using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Entities.Business;
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

            // Start with BaseDiscount
            // Then wrap with decorators
            IDiscount discount = new BaseDiscount();

            // Example: Auto apply free shipping for orders > 500k
            discount = new FreeShippingDiscountDecorator(discount, shippingCost: 50000, minItemCount: 0);

            return discount;
        }

        public IDiscount BuildDiscount(params IDiscount[] discounts)
        {
            if (discounts == null || discounts.Length == 0)
                return new BaseDiscount();

            _logger.LogInfo($"Building discount chain with {discounts.Length} discounts", "DiscountService");

            // Start from first discount
            IDiscount result = discounts[0];

            // Wrap sequentially with other discounts
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

        public IDiscount BuildDiscountFromCodes(IEnumerable<string> discountCodes)
        {
            if (discountCodes == null)
            {
                return new BaseDiscount();
            }

            IDiscount chain = new BaseDiscount();

            foreach (var rawCode in discountCodes)
            {
                if (string.IsNullOrWhiteSpace(rawCode))
                {
                    continue;
                }

                var code = rawCode.Trim().ToLowerInvariant();

                if (code.StartsWith("percentage_"))
                {
                    if (decimal.TryParse(code.Replace("percentage_", string.Empty), out var percent))
                    {
                        chain = new PercentageDiscountDecorator(chain, percent);
                    }

                    continue;
                }

                if (code.StartsWith("fixed_"))
                {
                    if (decimal.TryParse(code.Replace("fixed_", string.Empty), out var amount))
                    {
                        chain = new FixedAmountDiscountDecorator(chain, amount, 0);
                    }

                    continue;
                }

                if (code == "free_shipping")
                {
                    chain = new FreeShippingDiscountDecorator(chain, shippingCost: 50000, minItemCount: 0);
                    continue;
                }

                if (code.StartsWith("loyalty_points_"))
                {
                    if (int.TryParse(code.Replace("loyalty_points_", string.Empty), out var points))
                    {
                        chain = new LoyaltyPointsDiscountDecorator(chain, points, pointsValue: 1000);
                    }

                    continue;
                }

                if (code.StartsWith("bundle_"))
                {
                    var parts = code.Split('_');
                    if (parts.Length == 3
                        && int.TryParse(parts[1], out var requiredCount)
                        && decimal.TryParse(parts[2], out var bundlePercent))
                    {
                        chain = new BundleDiscountDecorator(chain, requiredCount, bundlePercent);
                    }
                }
            }

            return chain;
        }

        public IDiscount BuildDiscountFromDiscountEntities(IEnumerable<Discount> discounts)
        {
            if (discounts == null)
            {
                return new BaseDiscount();
            }

            IDiscount chain = new BaseDiscount();

            foreach (var discount in discounts)
            {
                if (discount == null || !discount.CanBeUsed)
                {
                    continue;
                }

                var discountType = discount.DiscountType?.Trim().ToLowerInvariant();

                switch (discountType)
                {
                    case "percentage":
                        chain = new PercentageDiscountDecorator(
                            chain,
                            discount.DiscountValue,
                            discount.MinimumOrderAmount ?? 0);
                        break;

                    case "fixedamount":
                    case "fixed":
                        chain = new FixedAmountDiscountDecorator(
                            chain,
                            discount.DiscountValue,
                            discount.MinimumOrderAmount ?? 0);
                        break;

                    case "freeshipping":
                    case "free_shipping":
                        chain = new FreeShippingDiscountDecorator(
                            chain,
                            shippingCost: discount.DiscountValue > 0 ? discount.DiscountValue : 50000,
                            minItemCount: 0);
                        break;
                }
            }

            return chain;
        }
    }
}
