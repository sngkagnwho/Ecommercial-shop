using System.ComponentModel.DataAnnotations;

namespace mtkpm.Admin.Models.Payment
{
    /// <summary>
    /// Payment method response from API
    /// </summary>
    public class PaymentMethodViewModel
    {
        /// <summary>
        /// Unique payment method ID
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// System code (credit_card, debit_card, bank_transfer, paypal, cod, mobile_wallet)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        /// <summary>
        /// Display name (Vietnamese)
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Description of the payment method
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// Icon emoji or identifier
        /// </summary>
        [StringLength(50)]
        public string Icon { get; set; }

        /// <summary>
        /// Whether method is currently active/available
        /// </summary>
        [Required]
        public bool IsActive { get; set; }

        /// <summary>
        /// Display priority (1 = highest)
        /// </summary>
        [Required]
        [Range(0, 999)]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Transaction fee percentage (0-100)
        /// </summary>
        [Required]
        [Range(0, 100)]
        public decimal TransactionFeePercentage { get; set; }

        /// <summary>
        /// Fixed transaction fee in VND
        /// </summary>
        [Required]
        [Range(0, 9999999)]
        public decimal TransactionFeeFixed { get; set; }

        /// <summary>
        /// Minimum transaction amount in VND
        /// </summary>
        [Required]
        [Range(0, 999999999999)]
        public decimal MinAmount { get; set; }

        /// <summary>
        /// Maximum transaction amount in VND
        /// </summary>
        [Required]
        [Range(0, 999999999999)]
        public decimal MaxAmount { get; set; }

        /// <summary>
        /// Human-readable processing time
        /// </summary>
        [StringLength(100)]
        public string ProcessingTime { get; set; }

        /// <summary>
        /// Requirements for using this method
        /// </summary>
        [StringLength(500)]
        public string Requirements { get; set; }

        /// <summary>
        /// Supported providers (comma-separated)
        /// </summary>
        [StringLength(500)]
        public string SupportedProviders { get; set; }

        /// <summary>
        /// Supported areas (comma-separated)
        /// </summary>
        [StringLength(500)]
        public string SupportedAreas { get; set; }

        /// <summary>
        /// Admin notes
        /// </summary>
        [StringLength(1000)]
        public string AdminNotes { get; set; }

        /// <summary>
        /// When the method was created
        /// </summary>
        public DateTime CreateAt { get; set; }

        /// <summary>
        /// Calculated: Is fee high?
        /// </summary>
        public bool IsHighFee => TransactionFeePercentage >= 2.5m;

        /// <summary>
        /// Calculated: Display fee summary
        /// </summary>
        public string FeeSummary => TransactionFeePercentage > 0 || TransactionFeeFixed > 0
            ? $"{TransactionFeePercentage}% + {TransactionFeeFixed:N0}đ"
            : "No fee";

        /// <summary>
        /// Calculated: Amount range summary
        /// </summary>
        public string AmountRange => $"{MinAmount:N0}đ - {MaxAmount:N0}đ";
    }

    /// <summary>
    /// Create payment method request
    /// </summary>
    public class CreatePaymentMethodViewModel
    {
        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Icon { get; set; }

        [Range(0, 999)]
        public int DisplayOrder { get; set; } = 0;

        [Range(0, 100)]
        public decimal TransactionFeePercentage { get; set; } = 0;

        [Range(0, 9999999)]
        public decimal TransactionFeeFixed { get; set; } = 0;

        [Range(0, 999999999999)]
        public decimal MinAmount { get; set; } = 0;

        [Range(0, 999999999999)]
        public decimal MaxAmount { get; set; } = 9999999999;

        [StringLength(100)]
        public string ProcessingTime { get; set; } = "Tức thì";

        [StringLength(500)]
        public string Requirements { get; set; }

        [StringLength(500)]
        public string SupportedProviders { get; set; }

        [StringLength(500)]
        public string SupportedAreas { get; set; }

        [StringLength(1000)]
        public string AdminNotes { get; set; }
    }

    /// <summary>
    /// Update payment method request
    /// </summary>
    public class UpdatePaymentMethodViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Icon { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        [Range(0, 999)]
        public int DisplayOrder { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal TransactionFeePercentage { get; set; }

        [Required]
        [Range(0, 9999999)]
        public decimal TransactionFeeFixed { get; set; }

        [Required]
        [Range(0, 999999999999)]
        public decimal MinAmount { get; set; }

        [Required]
        [Range(0, 999999999999)]
        public decimal MaxAmount { get; set; }

        [Required]
        [StringLength(100)]
        public string ProcessingTime { get; set; }

        [StringLength(500)]
        public string Requirements { get; set; }

        [StringLength(500)]
        public string SupportedProviders { get; set; }

        [StringLength(500)]
        public string SupportedAreas { get; set; }

        [StringLength(1000)]
        public string AdminNotes { get; set; }
    }

    /// <summary>
    /// Payment methods statistics for dashboard
    /// </summary>
    public class PaymentStatisticsViewModel
    {
        public int TotalMethods { get; set; }
        public int ActiveMethods { get; set; }
        public int InactiveMethods { get; set; }
        public decimal HighestFeePercentage { get; set; }
        public decimal AverageFeePercentage { get; set; }
    }
}
