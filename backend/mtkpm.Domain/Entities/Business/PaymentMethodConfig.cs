using mtkpm.Domain.Entities.Base;

namespace mtkpm.Domain.Entities.Business
{
    /// <summary>
    /// Entity qu?n lý các ph??ng th?c thanh toán
    /// Cho phép Admin kích ho?t/vô hi?u hóa các ph??ng th?c
    /// </summary>
    public class PaymentMethodConfig : SoftDeleteEntity
    {
        /// <summary>
        /// Mã ph??ng th?c: CreditCard, BankTransfer, COD, PayPal
        /// </summary>
        public string Code { get; set; } = null!;

        /// <summary>
        /// Tên ph??ng th?c hi?n th?
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Mô t? ph??ng th?c
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Icon emoji ho?c URL
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// Ph??ng th?c có kích ho?t không?
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Th? t? hi?n th? (1 = hi?n th? ??u tiên)
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Phí giao d?ch (%)
        /// Ví d?: 2.5 = 2.5% phí
        /// </summary>
        public decimal TransactionFeePercentage { get; set; } = 0m;

        /// <summary>
        /// Phí giao d?ch c? ??nh (?)
        /// </summary>
        public decimal TransactionFeeFixed { get; set; } = 0m;

        /// <summary>
        /// S? ti?n t?i thi?u ?? s? d?ng
        /// </summary>
        public decimal MinAmount { get; set; } = 0m;

        /// <summary>
        /// S? ti?n t?i ?a
        /// </summary>
        public decimal MaxAmount { get; set; } = 9999999999m;

        /// <summary>
        /// Th?i gian x? lý (vd: "T?c th?i", "1-3 ngày làm vi?c")
        /// </summary>
        public string ProcessingTime { get; set; } = "T?c th?i";

        /// <summary>
        /// Các yêu c?u ?? s? d?ng (JSON array)
        /// </summary>
        public string? Requirements { get; set; }

        /// <summary>
        /// Các ngân hàng/card h? tr?
        /// </summary>
        public string? SupportedProviders { get; set; }

        /// <summary>
        /// Khu v?c h? tr?
        /// </summary>
        public string? SupportedAreas { get; set; }

        /// <summary>
        /// Ghi chú t? Admin
        /// </summary>
        public string? AdminNotes { get; set; }

        /// <summary>
        /// User t?o c?u hình
        /// </summary>
        public int CreatedByUserId { get; set; }

        /// <summary>
        /// URL webhook cho webhook integration
        /// </summary>
        public string? WebhookUrl { get; set; }

        /// <summary>
        /// API Key/Token cho third-party integration
        /// Nên ???c encrypt
        /// </summary>
        public string? ApiKey { get; set; }

        /// <summary>
        /// C?u hình JSON cho ph??ng th?c (l?u thêm settings)
        /// </summary>
        public string? Configuration { get; set; }
    }
}
