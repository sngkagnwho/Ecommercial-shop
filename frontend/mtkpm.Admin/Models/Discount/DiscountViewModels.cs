using System.ComponentModel.DataAnnotations;

namespace mtkpm.Admin.Models.Discount
{
    /// <summary>
    /// Available discount code information
    /// </summary>
    public class DiscountCodeDto
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(100)]
        public string Example { get; set; }
    }

    /// <summary>
    /// Discount calculation result
    /// </summary>
    public class CalculateDiscountResponse
    {
        public decimal OriginalAmount { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal FinalAmount { get; set; }

        public List<AppliedDiscountInfo> AppliedDiscounts { get; set; } = new();

        public string Message { get; set; }

        public bool IsSuccess { get; set; }
    }

    /// <summary>
    /// Information about an applied discount
    /// </summary>
    public class AppliedDiscountInfo
    {
        public string DiscountCode { get; set; }

        public string DiscountName { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal AdjustedAmount { get; set; }

        public int AppliedOrder { get; set; }

        public string Description { get; set; }
    }

    /// <summary>
    /// Discount tester request
    /// </summary>
    public class TestDiscountRequest
    {
        [Required]
        public decimal CartAmount { get; set; }

        public List<string> DiscountCodes { get; set; } = new();
    }

    /// <summary>
    /// Discount tester result display model
    /// </summary>
    public class DiscountTestResultViewModel
    {
        public decimal CartAmount { get; set; }

        public List<string> AppliedCodes { get; set; } = new();

        public decimal TotalSavings { get; set; }

        public decimal FinalAmount { get; set; }

        public double SavingsPercentage => CartAmount > 0 ? (double)(TotalSavings * 100 / CartAmount) : 0;

        public List<AppliedDiscountInfo> DiscountBreakdown { get; set; } = new();

        public bool Success { get; set; }

        public string Message { get; set; }

        public DateTime TestedAt { get; set; }
    }

    /// <summary>
    /// Decorator Pattern guide and documentation
    /// </summary>
    public class DiscountPatternGuideViewModel
    {
        public string Title => "Decorator Pattern - Chiết Khấu Xếp Chồng";

        public string Description => @"
Hệ thống chiết khấu sử dụng Decorator Pattern cho phép áp dụng 
nhiều código chiết khấu cùng lúc theo thứ tự (stacking).
";

        public List<string> AvailablePatterns => new()
        {
            "Percentage Discount - Giảm theo phần trăm",
            "Fixed Amount Discount - Giảm số tiền cố định",
            "Free Shipping - Miễn phí vận chuyển",
            "Loyalty Points - Sử dụng điểm thành viên",
            "Bundle Discount - Chiết khấu combo"
        };

        public List<DiscountStackingExample> Examples => new()
        {
            new DiscountStackingExample
            {
                Name = "Ví dụ 1: Một chiết khấu",
                Codes = new() { "percentage_10" },
                Description = "Giảm 10% giá hàng",
                Calculation = "100.000đ × 10% = 10.000đ tiết kiệm"
            },
            new DiscountStackingExample
            {
                Name = "Ví dụ 2: Xếp hai chiết khấu",
                Codes = new() { "percentage_10", "free_shipping" },
                Description = "Giảm 10% + Miễn phí ship",
                Calculation = "Giảm 10.000đ + Tiết kiệm 50.000đ ship = 60.000đ tổng tiết kiệm"
            },
            new DiscountStackingExample
            {
                Name = "Ví dụ 3: Xếp ba chiết khấu",
                Codes = new() { "percentage_20", "loyalty_points_50", "free_shipping" },
                Description = "Giảm 20% + Dùng 50 điểm + Miễn phí ship",
                Calculation = "Giảm 20.000đ + 50.000đ điểm + Miễn phí ship = Tiết kiệm lớn"
            }
        };

        public string KeyBenefits => @"
✓ Linh hoạt: Kết hợp bất kỳ chiết khấu nào
✓ Không bùng nổ class: Không cần tạo nhiều class kết hợp
✓ Open/Closed: Dễ thêm chiết khấu mới
✓ Runtime Composition: Quyết định chiết khấu lúc chạy
";
    }

    /// <summary>
    /// Example of discount stacking
    /// </summary>
    public class DiscountStackingExample
    {
        public string Name { get; set; }

        public List<string> Codes { get; set; } = new();

        public string Description { get; set; }

        public string Calculation { get; set; }
    }

    // Admin CRUD Models
    
    public class DiscountViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public string DiscountType { get; set; } = "Percentage"; // Percentage, Fixed, FreeShipping, LoyaltyPoints, Bundle
        public decimal DiscountValue { get; set; }
        public decimal? MinimumOrderAmount { get; set; }
        public decimal? MaximumDiscountAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? MaxUsageCount { get; set; }
        public int UsedCount { get; set; }
        public int? MaxUsagePerUser { get; set; }
        public decimal? BudgetLimit { get; set; }
        public decimal BudgetUsed { get; set; }
        public bool IsActive { get; set; }
        public bool IsNewUserOnly { get; set; }
        public bool IsStackable { get; set; }
        public bool IsExpired { get; set; }
        public bool IsBudgetExhausted { get; set; }
        public bool IsUsageLimitReached { get; set; }
        public bool CanBeUsed { get; set; }
        public DateTime CreateAt { get; set; }
    }

    public class CreateDiscountViewModel
    {
        [Required]
        public string Code { get; set; } = "";

        [Required]
        public string Name { get; set; } = "";

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public string DiscountType { get; set; } = "Percentage";

        [Required]
        public decimal DiscountValue { get; set; }

        public decimal? MinimumOrderAmount { get; set; }
        public decimal? MaximumDiscountAmount { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public int? MaxUsageCount { get; set; }
        public int? MaxUsagePerUser { get; set; }
        public decimal? BudgetLimit { get; set; }
        public bool IsNewUserOnly { get; set; } = false;
        public bool IsStackable { get; set; } = true;
        public string? ApplicableCategories { get; set; }
        public string? ApplicableProducts { get; set; }
        public string? AdminNotes { get; set; }
    }

    public class UpdateDiscountViewModel
    {
        [Required]
        public string Name { get; set; } = "";

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public string DiscountType { get; set; } = "";

        [Required]
        public decimal DiscountValue { get; set; }

        public decimal? MinimumOrderAmount { get; set; }
        public decimal? MaximumDiscountAmount { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public int? MaxUsageCount { get; set; }
        public int? MaxUsagePerUser { get; set; }
        public decimal? BudgetLimit { get; set; }
        public bool IsActive { get; set; }
        public bool IsNewUserOnly { get; set; }
        public bool IsStackable { get; set; }
        public string? ApplicableCategories { get; set; }
        public string? ApplicableProducts { get; set; }
        public string? AdminNotes { get; set; }
    }

    public class DiscountStatisticsViewModel
    {
        public int TotalDiscounts { get; set; }
        public int ActiveDiscounts { get; set; }
        public int ExpiredDiscounts { get; set; }
        public int BudgetExhaustedDiscounts { get; set; }
        public decimal TotalBudgetLimit { get; set; }
        public decimal TotalBudgetUsed { get; set; }
        public int TotalUsageCount { get; set; }
        public decimal TotalDiscountAmount { get; set; }
    }
}
