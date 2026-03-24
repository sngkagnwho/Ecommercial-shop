using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Enums.Business;

namespace mtkpm.Application.Common.Interfaces.Services
{
    /// <summary>
    /// Payment Service Interface - ??nh ngh?a payment processing
    /// Implement t? Infrastructure Layer
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// X? lý thanh toán v?i payment method
        /// </summary>
        Task<PaymentResult> ProcessPaymentAsync(
            PaymentMethodType paymentType,
            decimal amount,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Hoàn ti?n
        /// </summary>
        Task<bool> RefundPaymentAsync(
            PaymentMethodType paymentType,
            string transactionId,
            decimal amount,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Ki?m tra tr?ng thái thanh toán
        /// </summary>
        Task<PaymentStatus> GetPaymentStatusAsync(
            PaymentMethodType paymentType,
            string transactionId,
            CancellationToken cancellationToken = default);
    }
}
