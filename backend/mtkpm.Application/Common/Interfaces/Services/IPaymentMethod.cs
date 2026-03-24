using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Enums.Business;

namespace mtkpm.Application.Common.Interfaces.Services
{
    public interface IPaymentMethod
    {
        /// <summary>
        /// Xác th?c thông tin thanh toán
        /// </summary>
        Task<bool> ValidateAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// X? lý thanh toán
        /// </summary>
        Task<PaymentResult> ProcessPaymentAsync(decimal amount, CancellationToken cancellationToken = default);

        /// <summary>
        /// Hoàn ti?n
        /// </summary>
        Task<bool> RefundAsync(string transactionId, decimal amount, CancellationToken cancellationToken = default);

        /// <summary>
        /// Ki?m tra tr?ng thái thanh toán
        /// </summary>
        Task<PaymentStatus> CheckPaymentStatusAsync(string transactionId, CancellationToken cancellationToken = default);
    }

    public class PaymentResult
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; }
        public string Message { get; set; }
        public DateTime TransactionDate { get; set; }
        public PaymentStatus Status { get; set; }
    }

    public enum PaymentStatus
    {
        Pending = 1,
        Completed = 2,
        Failed = 3,
        Cancelled = 4,
        Refunded = 5
    }
}
