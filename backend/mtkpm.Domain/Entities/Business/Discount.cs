using mtkpm.Domain.Entities.Base;

namespace mtkpm.Domain.Entities.Business
{
    /// <summary>
    /// Entity qu?n lý chi?t kh?u
    /// </summary>
    public class Discount : SoftDeleteEntity
    {
        /// <summary>
        /// Mã chi?t kh?u (vd: "SUMMER2024", "NEW50")
        /// </summary>
        public string Code { get; set; } = null!;

        /// <summary>
        /// Tên chi?t kh?u (vd: "Hè 2024", "Chi?t kh?u m?i")
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Mô t? chi?t kh?u
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Lo?i chi?t kh?u: Percentage (ph?n tr?m), FixedAmount (s? ti?n c? ??nh), FreeShipping (mi?n phí v?n chuy?n)
        /// </summary>
        public string DiscountType { get; set; } = "Percentage"; // "Percentage", "FixedAmount", "FreeShipping"

        /// <summary>
        /// Giá tr? chi?t kh?u (vd: 10 n?u là ph?n tr?m, 50000 n?u là s? ti?n)
        /// </summary>
        public decimal DiscountValue { get; set; }

        /// <summary>
        /// Giá tr? t?i thi?u ??n hàng ?? áp d?ng chi?t kh?u
        /// </summary>
        public decimal? MinimumOrderAmount { get; set; }

        /// <summary>
        /// Giá tr? t?i ?a chi?t kh?u có th? ???c áp d?ng (vd: gi?m t?i ?a 500k)
        /// </summary>
        public decimal? MaximumDiscountAmount { get; set; }

        /// <summary>
        /// Ngày b?t ??u áp d?ng chi?t kh?u
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Ngày k?t thúc áp d?ng chi?t kh?u
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// S? l?n t?i ?a có th? s? d?ng chi?t kh?u này (null = unlimited)
        /// </summary>
        public int? MaxUsageCount { get; set; }

        /// <summary>
        /// S? l?n ?ã s? d?ng chi?t kh?u
        /// </summary>
        public int UsedCount { get; set; } = 0;

        /// <summary>
        /// S? l?n t?i ?a m?i user có th? s? d?ng (null = unlimited)
        /// </summary>
        public int? MaxUsagePerUser { get; set; }

        /// <summary>
        /// Ngân sách t?i ?a cho chi?t kh?u này (t?ng ti?n gi?m t?i ?a)
        /// </summary>
        public decimal? BudgetLimit { get; set; }

        /// <summary>
        /// T?ng ti?n ?ã ???c gi?m thông qua chi?t kh?u này
        /// </summary>
        public decimal BudgetUsed { get; set; } = 0;

        /// <summary>
        /// Chi?t kh?u có ho?t ??ng không?
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Danh sách các danh m?c có th? áp d?ng (null = t?t c?)
        /// L?u d??i d?ng chu?i JSON ho?c CSV: "1,2,3"
        /// </summary>
        public string? ApplicableCategories { get; set; }

        /// <summary>
        /// Danh sách các s?n ph?m có th? áp d?ng (null = t?t c?)
        /// L?u d??i d?ng chu?i JSON ho?c CSV: "1,2,3"
        /// </summary>
        public string? ApplicableProducts { get; set; }

        /// <summary>
        /// Chi?t kh?u ch? dành cho user m?i (ch?a mua hàng)
        /// </summary>
        public bool IsNewUserOnly { get; set; } = false;

        /// <summary>
        /// Chi?t kh?u có th? k?t h?p v?i các chi?t kh?u khác không
        /// </summary>
        public bool IsStackable { get; set; } = true;

        /// <summary>
        /// User ?ã t?o chi?t kh?u này
        /// </summary>
        public int CreatedByUserId { get; set; }

        /// <summary>
        /// Ghi chú t? Admin
        /// </summary>
        public string? AdminNotes { get; set; }

        /// <summary>
        /// L?ch s? s? d?ng chi?t kh?u
        /// </summary>
        public ICollection<DiscountUsageHistory> UsageHistories { get; set; } = new List<DiscountUsageHistory>();

        /// <summary>
        /// Ki?m tra xem chi?t kh?u có còn h?n không
        /// </summary>
        public bool IsExpired => DateTime.UtcNow > EndDate;

        /// <summary>
        /// Ki?m tra xem chi?t kh?u ?ã h?t ngân sách không
        /// </summary>
        public bool IsBudgetExhausted => BudgetLimit.HasValue && BudgetUsed >= BudgetLimit.Value;

        /// <summary>
        /// Ki?m tra xem chi?t kh?u ?ã ??t s? l?n s? d?ng t?i ?a không
        /// </summary>
        public bool IsUsageLimitReached => MaxUsageCount.HasValue && UsedCount >= MaxUsageCount.Value;

        /// <summary>
        /// Ki?m tra xem chi?t kh?u có th? s? d?ng hi?n t?i không
        /// </summary>
        public bool CanBeUsed => IsActive && !IsExpired && !IsBudgetExhausted && !IsUsageLimitReached;
    }
}
