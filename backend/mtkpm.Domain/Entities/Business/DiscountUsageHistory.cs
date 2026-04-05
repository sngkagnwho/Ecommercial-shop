using mtkpm.Domain.Entities.Base;

namespace mtkpm.Domain.Entities.Business
{
    /// <summary>
    /// L?ch s? s? d?ng chi?t kh?u
    /// </summary>
    public class DiscountUsageHistory : BaseEntity
    {
        /// <summary>
        /// Chi?t kh?u ???c s? d?ng
        /// </summary>
        public int DiscountId { get; set; }
        public Discount Discount { get; set; } = null!;

        /// <summary>
        /// User s? d?ng chi?t kh?u
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Order mà chi?t kh?u ???c áp d?ng
        /// </summary>
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        /// <summary>
        /// S? ti?n ???c gi?m
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Ngày s? d?ng
        /// </summary>
        public DateTime UsedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// IP address khi s? d?ng
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// Ghi chú b? sung
        /// </summary>
        public string? Notes { get; set; }
    }
}
