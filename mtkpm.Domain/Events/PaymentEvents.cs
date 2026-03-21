namespace mtkpm.Domain.Events
{
    /// <summary>
    /// Event khi thanh toán thành công
    /// </summary>
    public class PaymentCompletedEvent : DomainEvent
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }

        public PaymentCompletedEvent(int orderId, int userId, string transactionId, decimal amount, string paymentMethod)
        {
            AggregateId = Guid.NewGuid();
            OrderId = orderId;
            UserId = userId;
            TransactionId = transactionId;
            Amount = amount;
            PaymentMethod = paymentMethod;
        }
    }

    /// <summary>
    /// Event khi thanh toán th?t b?i
    /// </summary>
    public class PaymentFailedEvent : DomainEvent
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }

        public PaymentFailedEvent(int orderId, int userId, decimal amount, string reason)
        {
            AggregateId = Guid.NewGuid();
            OrderId = orderId;
            UserId = userId;
            Amount = amount;
            Reason = reason;
        }
    }

    /// <summary>
    /// Event khi hoàn ti?n
    /// </summary>
    public class PaymentRefundedEvent : DomainEvent
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string TransactionId { get; set; }
        public decimal RefundAmount { get; set; }
        public string Reason { get; set; }

        public PaymentRefundedEvent(int orderId, int userId, string transactionId, decimal refundAmount, string reason)
        {
            AggregateId = Guid.NewGuid();
            OrderId = orderId;
            UserId = userId;
            TransactionId = transactionId;
            RefundAmount = refundAmount;
            Reason = reason;
        }
    }
}
