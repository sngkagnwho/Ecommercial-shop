using MediatR;
using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Repositories;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Application.Features.Products.Commands.CalculatePrice
{
    public class CalculatePriceCommandHandler : IRequestHandler<CalculatePriceCommand, CalculatePriceResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPricingService _pricingService;
        private readonly ILoggerService _logger;

        public CalculatePriceCommandHandler(
            IUnitOfWork unitOfWork,
            IPricingService pricingService,
            ILoggerService logger)
        {
            _unitOfWork = unitOfWork;
            _pricingService = pricingService;
            _logger = logger;
        }

        public async Task<CalculatePriceResponse> Handle(
            CalculatePriceCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInfo($"Calculating price for Product {request.ProductId} - Qty: {request.Quantity}", "PricingHandler");

            // Get product
            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId, cancellationToken);
            if (product == null)
            {
                throw new KeyNotFoundException($"S?n ph?m có ID {request.ProductId} không t?n t?i");
            }

            var baseTotalPrice = product.Price * request.Quantity;

            // Create pricing context
            var context = new PricingContext
            {
                UserId = request.UserId,
                UserTier = request.UserTier,
                CurrentDate = DateTime.UtcNow,
                IsVipMember = !string.IsNullOrEmpty(request.UserTier)
            };

            // Calculate price
            decimal finalPrice;

            if (string.IsNullOrEmpty(request.PricingStrategy))
            {
                // Auto select best price
                finalPrice = _pricingService.CalculateBestPrice(product, request.Quantity, context);
            }
            else
            {
                // Use PricingService to get strategy and calculate price
                var strategy = _pricingService.GetStrategyByName(request.PricingStrategy);
                
                if (strategy == null)
                {
                    throw new ArgumentException($"Chi?n l??c ??nh giá '{request.PricingStrategy}' không tìm th?y. Các tùy ch?n h?p l?: 'regular', 'bulk', 'seasonal', 'vip'");
                }

                finalPrice = _pricingService.CalculatePrice(product, request.Quantity, strategy, context);
            }

            var savingsAmount = baseTotalPrice - finalPrice;
            var savingsPercent = baseTotalPrice > 0 ? (savingsAmount / baseTotalPrice) * 100 : 0;

            return new CalculatePriceResponse
            {
                ProductId = product.Id,
                ProductName = product.Name,
                BasePrice = product.Price,
                Quantity = request.Quantity,
                BaseTotalPrice = baseTotalPrice,
                AppliedStrategy = request.PricingStrategy ?? "Auto (Best Price)",
                FinalPrice = finalPrice,
                SavingsAmount = savingsAmount,
                SavingsPercent = savingsPercent
            };
        }
    }
}
