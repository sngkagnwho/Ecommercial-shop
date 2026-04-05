namespace mtkpm.Application.Common.DTOs.Pricing
{
    public class PricingRuleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string RuleType { get; set; } = null!;
        public string? Description { get; set; }
        public decimal RuleValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int Priority { get; set; }
        public string? ApplicableProductIds { get; set; }
        public string? ApplicableCategoryIds { get; set; }
        public bool IsExpired { get; set; }
        public bool CanBeUsed { get; set; }
        public DateTime CreateAt { get; set; }
    }

    public class CreatePricingRuleDto
    {
        public string Name { get; set; } = null!;
        public string RuleType { get; set; } = null!;
        public string? Description { get; set; }
        public string RuleCondition { get; set; } = null!;
        public decimal RuleValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; } = 0;
        public string? ApplicableProductIds { get; set; }
        public string? ApplicableCategoryIds { get; set; }
        public string? AdminNotes { get; set; }
    }

    public class UpdatePricingRuleDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string RuleCondition { get; set; } = null!;
        public decimal RuleValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int Priority { get; set; }
        public string? ApplicableProductIds { get; set; }
        public string? ApplicableCategoryIds { get; set; }
        public string? AdminNotes { get; set; }
    }

    public class PricingStatisticsDto
    {
        public int TotalRules { get; set; }
        public int ActiveRules { get; set; }
        public int ExpiredRules { get; set; }
        public decimal AverageDiscount { get; set; }
    }
}
