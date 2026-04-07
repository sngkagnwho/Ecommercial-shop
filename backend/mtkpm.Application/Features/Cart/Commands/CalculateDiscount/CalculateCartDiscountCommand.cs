using MediatR;
using mtkpm.Application.Common.Interfaces.Services;

namespace mtkpm.Application.Features.Cart.Commands.CalculateDiscount
{
    public class CalculateCartDiscountCommand : IRequest<CalculateCartDiscountResponse>
    {
        public int UserId { get; set; }
        
        /// <summary>
        /// Danh sách discount type codes ?? áp d?ng
        /// "percentage_10" = 10% discount
        /// "fixed_100000" = gi?m 100k
        /// "free_shipping" = mi?n phí ship
        /// "loyalty_points_50" = 50 ?i?m
        /// "bundle_3_15" = mua 3+ ???c -15%
        /// </summary>
        public List<string> DiscountCodes { get; set; } = new();
    }

    public class CalculateCartDiscountResponse
    {
        public int UserId { get; set; }
        public int TotalItems { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        // Backward-compatible alias for clients using `discountAmount`
        public decimal DiscountAmount => TotalDiscountAmount;
        public decimal FinalAmount { get; set; }
        public decimal SavingsPercent { get; set; }
        public List<string> AppliedDiscounts { get; set; } = new();
        public string Message { get; set; }
    }
}
