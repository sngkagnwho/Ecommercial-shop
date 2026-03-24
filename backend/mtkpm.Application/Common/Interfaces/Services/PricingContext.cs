namespace mtkpm.Application.Common.Interfaces.Services
{
    /// <summary>
    /// Context dùng cho pricing strategies
    /// Ch?a thông tin v? user, ngày, mùa ?? strategies quy?t ??nh giá
    /// </summary>
    public class PricingContext
    {
        public int? UserId { get; set; }
        public bool IsVipMember { get; set; }
        public DateTime CurrentDate { get; set; } = DateTime.UtcNow;
        public string? UserTier { get; set; } // Bronze, Silver, Gold, Platinum
        public int? SeasonId { get; set; }
    }
}
