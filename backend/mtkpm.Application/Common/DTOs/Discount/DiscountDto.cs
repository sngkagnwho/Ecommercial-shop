namespace mtkpm.Application.Common.DTOs.Discount
{
    public class DiscountDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string DiscountType { get; set; } = null!;
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

    public class CreateDiscountDto
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string DiscountType { get; set; } = "Percentage";
        public decimal DiscountValue { get; set; }
        public decimal? MinimumOrderAmount { get; set; }
        public decimal? MaximumDiscountAmount { get; set; }
        public DateTime StartDate { get; set; }
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

    public class UpdateDiscountDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string DiscountType { get; set; } = null!;
        public decimal DiscountValue { get; set; }
        public decimal? MinimumOrderAmount { get; set; }
        public decimal? MaximumDiscountAmount { get; set; }
        public DateTime StartDate { get; set; }
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

    public class DiscountUsageHistoryDto
    {
        public int Id { get; set; }
        public int DiscountId { get; set; }
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime UsedAt { get; set; }
        public string? IpAddress { get; set; }
    }

    public class DiscountStatisticsDto
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
