namespace mtkpm.Application.Common.DTOs.Payment
{
    public class PaymentStatusInfoDto
    {
        public int OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string TransactionId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
