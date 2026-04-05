using MediatR;
using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Repositories;
using mtkpm.Application.Common.Interfaces.Services;

namespace mtkpm.Application.Features.Cart.Commands.CalculateDiscount
{
    public class CalculateCartDiscountCommandHandler : IRequestHandler<CalculateCartDiscountCommand, CalculateCartDiscountResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDiscountRepository _discountRepository;
        private readonly IDiscountService _discountService;
        private readonly ILoggerService _logger;

        public CalculateCartDiscountCommandHandler(
            IUnitOfWork unitOfWork,
            IDiscountRepository discountRepository,
            IDiscountService discountService,
            ILoggerService logger)
        {
            _unitOfWork = unitOfWork;
            _discountRepository = discountRepository;
            _discountService = discountService;
            _logger = logger;
        }

        public async Task<CalculateCartDiscountResponse> Handle(
            CalculateCartDiscountCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInfo($"Calculating cart discount for User {request.UserId}", "DiscountHandler");

            // Get user's cart
            var cartItems = await _unitOfWork.CartItems.FindAsync(
                ci => ci.UserId == request.UserId,
                cancellationToken);

            if (!cartItems.Any())
            {
                return new CalculateCartDiscountResponse
                {
                    UserId = request.UserId,
                    TotalItems = 0,
                    OriginalAmount = 0,
                    Message = "Cart is empty"
                };
            }

            // Convert to CartDto
            var cartDto = new mtkpm.Application.Common.DTOs.Cart.CartDto
            {
                UserId = request.UserId,
                TotalItems = cartItems.Count(),
                TotalAmount = cartItems.Sum(ci => ci.Product?.Price * ci.Quantity ?? 0),
                Items = cartItems.Select(ci => new mtkpm.Application.Common.DTOs.Cart.CartItemDto
                {
                    Id = ci.Id,
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity
                }).ToList()
            };

            var discount = _discountService.GetDefaultDiscounts();
            var appliedDiscounts = new List<string>();

            var requestedCodes = request.DiscountCodes
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (requestedCodes.Any())
            {
                var activeDiscounts = await _discountRepository.GetActiveDiscountsAsync();
                var activeDiscountMap = activeDiscounts.ToDictionary(d => d.Code, StringComparer.OrdinalIgnoreCase);

                var selectedDiscounts = requestedCodes
                    .Where(code => activeDiscountMap.ContainsKey(code))
                    .Select(code => activeDiscountMap[code])
                    .Where(d => d.CanBeUsed)
                    .ToList();

                if (selectedDiscounts.Any())
                {
                    discount = _discountService.BuildDiscountFromDiscountEntities(selectedDiscounts);
                    appliedDiscounts = selectedDiscounts.Select(d => d.Code).ToList();
                }
            }

            var discountInfo = _discountService.CalculateDiscountedPrice(cartDto, discount);
            if (!appliedDiscounts.Any())
            {
                appliedDiscounts = discountInfo.AppliedDiscounts;
            }

            return new CalculateCartDiscountResponse
            {
                UserId = request.UserId,
                TotalItems = cartDto.TotalItems,
                OriginalAmount = discountInfo.OriginalAmount,
                TotalDiscountAmount = discountInfo.DiscountAmount,
                FinalAmount = discountInfo.FinalAmount,
                SavingsPercent = discountInfo.SavingsPercent,
                AppliedDiscounts = appliedDiscounts,
                Message = $"Applied {appliedDiscounts.Count} discount(s). You saved {discountInfo.DiscountAmount:C}!"
            };
        }
    }
}
