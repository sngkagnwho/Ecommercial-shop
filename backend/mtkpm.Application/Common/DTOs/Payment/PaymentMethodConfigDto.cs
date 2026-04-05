namespace mtkpm.Application.Common.DTOs.Payment
{
    public class PaymentMethodConfigDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public decimal TransactionFeePercentage { get; set; }
        public decimal TransactionFeeFixed { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public string ProcessingTime { get; set; } = null!;
        public DateTime CreateAt { get; set; }
    }

    public class CreatePaymentMethodConfigDto
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public decimal TransactionFeePercentage { get; set; } = 0m;
        public decimal TransactionFeeFixed { get; set; } = 0m;
        public decimal MinAmount { get; set; } = 0m;
        public decimal MaxAmount { get; set; } = 9999999999m;
        public string ProcessingTime { get; set; } = "T?c th?i";
        public string? Requirements { get; set; }
        public string? SupportedProviders { get; set; }
        public string? SupportedAreas { get; set; }
        public string? AdminNotes { get; set; }
    }

    public class UpdatePaymentMethodConfigDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public decimal TransactionFeePercentage { get; set; }
        public decimal TransactionFeeFixed { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public string ProcessingTime { get; set; } = null!;
        public string? Requirements { get; set; }
        public string? SupportedProviders { get; set; }
        public string? SupportedAreas { get; set; }
        public string? AdminNotes { get; set; }
    }

    public class PaymentStatisticsDto
    {
        public int TotalMethods { get; set; }
        public int ActiveMethods { get; set; }
        public decimal AverageFeePercentage { get; set; }
        public decimal AverageFeeFixed { get; set; }
    }
}
