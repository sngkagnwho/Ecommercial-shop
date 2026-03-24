namespace mtkpm.Admin.Models.Payment
{
    public class PaymentViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
        public decimal Amount { get; set; }
        public string Status { get; set; } = "";
        public DateTime PaymentDate { get; set; }
        public string? TransactionId { get; set; }
    }

    public class PaymentHistoryViewModel
    {
        public int TotalPayments { get; set; }
        public decimal TotalAmount { get; set; }
        public List<PaymentViewModel> Payments { get; set; } = new();
    }

    public class ProcessPaymentViewModel
    {
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; } = "";
        public decimal Amount { get; set; }
    }

    public class RefundPaymentViewModel
    {
        public int PaymentId { get; set; }
        public decimal RefundAmount { get; set; }
        public string Reason { get; set; } = "";
    }
}
