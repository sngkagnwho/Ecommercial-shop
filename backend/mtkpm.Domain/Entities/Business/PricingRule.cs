using mtkpm.Domain.Entities.Base;

namespace mtkpm.Domain.Entities.Business
{
    /// <summary>
    /// Entity qu?n lý các quy t?c ??nh giá
    /// Cho phép Admin t?o và c?p nh?t các chi?n l??c ??nh giá
    /// </summary>
    public class PricingRule : SoftDeleteEntity
    {
        /// <summary>
        /// Tên quy t?c ??nh giá (vd: "Bulk Discount", "VIP Pricing")
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Lo?i quy t?c: Regular, Bulk, Seasonal, VIP
        /// </summary>
        public string RuleType { get; set; } = null!;

        /// <summary>
        /// Mô t? chi ti?t quy t?c
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// ?i?u ki?n áp d?ng (JSON format)
        /// Ví d?: {"minQuantity": 10, "percentage": 10}
        /// </summary>
        public string RuleCondition { get; set; } = null!;

        /// <summary>
        /// Giá tr? áp d?ng (s? ho?c ph?n tr?m)
        /// Ví d?: 10 (có th? là 10% ho?c 10000? tùy lo?i)
        /// </summary>
        public decimal RuleValue { get; set; }

        /// <summary>
        /// Ngày b?t ??u áp d?ng
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Ngày k?t thúc áp d?ng
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Quy t?c có ho?t ??ng không?
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// ?u tiên áp d?ng (cao nh?t ???c áp d?ng tr??c)
        /// </summary>
        public int Priority { get; set; } = 0;

        /// <summary>
        /// Danh sách s?n ph?m áp d?ng (null = t?t c?)
        /// L?u d??i d?ng CSV: "1,2,3"
        /// </summary>
        public string? ApplicableProductIds { get; set; }

        /// <summary>
        /// Danh sách danh m?c áp d?ng (null = t?t c?)
        /// L?u d??i d?ng CSV: "1,2,3"
        /// </summary>
        public string? ApplicableCategoryIds { get; set; }

        /// <summary>
        /// Ghi chú t? Admin
        /// </summary>
        public string? AdminNotes { get; set; }

        /// <summary>
        /// User t?o quy t?c
        /// </summary>
        public int CreatedByUserId { get; set; }

        /// <summary>
        /// Ki?m tra xem quy t?c còn hi?u l?c không
        /// </summary>
        public bool IsExpired => DateTime.UtcNow > EndDate;

        /// <summary>
        /// Ki?m tra xem quy t?c có th? áp d?ng không
        /// </summary>
        public bool CanBeUsed => IsActive && !IsExpired;
    }
}
