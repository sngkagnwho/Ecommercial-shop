using MediatR;
using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Repositories;
using mtkpm.Application.Common.Interfaces.Services;

namespace mtkpm.Application.Features.Cart.Commands.CalculateDiscount
{
    public class CalculateCartDiscountCommandHandler : IRequestHandler<CalculateCartDiscountCommand, CalculateCartDiscountResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDiscountService _discountService;
        private readonly ILoggerService _logger;

        public CalculateCartDiscountCommandHandler(
            IUnitOfWork unitOfWork,
            IDiscountService discountService,
            ILoggerService logger)
        {
            _unitOfWork = unitOfWork;
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

            // Use DiscountService to parse and build discounts
            // For now, apply default discounts
            var discount = _discountService.GetDefaultDiscounts();

            // Tính giá sau discount
            var discountInfo = _discountService.CalculateDiscountedPrice(cartDto, discount);

            return new CalculateCartDiscountResponse
            {
                UserId = request.UserId,
                TotalItems = cartDto.TotalItems,
                OriginalAmount = discountInfo.OriginalAmount,
                TotalDiscountAmount = discountInfo.DiscountAmount,
                FinalAmount = discountInfo.FinalAmount,
                SavingsPercent = discountInfo.SavingsPercent,
                AppliedDiscounts = discountInfo.AppliedDiscounts,
                Message = $"Applied {discountInfo.AppliedDiscounts.Count} discount(s). You saved {discountInfo.DiscountAmount:C}!"
            };
        }
    }
}
