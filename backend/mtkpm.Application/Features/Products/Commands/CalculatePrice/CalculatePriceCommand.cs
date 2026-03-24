using MediatR;
using mtkpm.Application.Common.Interfaces.Services;

namespace mtkpm.Application.Features.Products.Commands.CalculatePrice
{
    public class CalculatePriceCommand : IRequest<CalculatePriceResponse>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string? PricingStrategy { get; set; } // "regular", "bulk", "seasonal", "vip"
        public int? UserId { get; set; }
        public string? UserTier { get; set; } // Bronze, Silver, Gold, Platinum
    }

    public class CalculatePriceResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal BasePrice { get; set; }
        public int Quantity { get; set; }
        public decimal BaseTotalPrice { get; set; }
        public string AppliedStrategy { get; set; }
        public decimal FinalPrice { get; set; }
        public decimal SavingsAmount { get; set; }
        public decimal SavingsPercent { get; set; }
    }
}
