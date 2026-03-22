using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Entities.Business;
using mtkpm.Domain.Events;

namespace mtkpm.Infrastructure.Services.Pricing
{
    /// <summary>
    /// Pricing Service Implementation - S? d?ng Strategy Pattern ?? tính giá
    /// </summary>
    public class PricingService : IPricingService
    {
        private readonly ILoggerService _logger;
        private readonly Dictionary<string, IPricingStrategy> _strategies;

        public PricingService(ILoggerService logger)
        {
            _logger = logger;
            
            // ??ng ký t?t c? strategies
            _strategies = new Dictionary<string, IPricingStrategy>
            {
                { "regular", new RegularPricingStrategy() },
                { "bulk", new BulkDiscountPricingStrategy(threshold: 10, discountPercent: 10m) },
                { "seasonal", new SeasonalPricingStrategy() },
                { "vip", new VIPMemberPricingStrategy() }
            };

            _logger.LogInfo("PricingService initialized with 4 pricing strategies", "Pricing");
        }

        public decimal CalculatePrice(Product product, int quantity, IPricingStrategy strategy, PricingContext context)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            
            if (strategy == null)
                throw new ArgumentNullException(nameof(strategy));

            _logger.LogInfo($"Calculating price using {strategy.StrategyName} - Product: {product.Name}, Qty: {quantity}", "Pricing");

            var finalPrice = strategy.CalculatePrice(product, quantity, context);

            _logger.LogInfo($"Final price: {finalPrice:C} (Strategy: {strategy.StrategyName})", "Pricing");

            return finalPrice;
        }

        public decimal CalculateBestPrice(Product product, int quantity, PricingContext context)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            _logger.LogInfo($"Finding best price for Product: {product.Name}, Qty: {quantity}", "Pricing");

            decimal bestPrice = product.Price * quantity;
            string bestStrategy = "Regular";

            // Th? t?t c? strategies và ch?n giá t?t nh?t (nh? nh?t) cho customer
            foreach (var strategy in _strategies.Values)
            {
                var price = strategy.CalculatePrice(product, quantity, context);
                if (price < bestPrice)
                {
                    bestPrice = price;
                    bestStrategy = strategy.StrategyName;
                }
            }

            _logger.LogInfo($"Best price found: {bestPrice:C} using {bestStrategy}", "Pricing");

            return bestPrice;
        }

        public IEnumerable<IPricingStrategy> GetAvailableStrategies()
        {
            return _strategies.Values;
        }

        /// <summary>
        /// L?y strategy theo tên
        /// </summary>
        public IPricingStrategy? GetStrategyByName(string strategyName)
        {
            if (string.IsNullOrEmpty(strategyName))
                return null;

            return _strategies.TryGetValue(strategyName.ToLower(), out var strategy) ? strategy : null;
        }
    }
}
