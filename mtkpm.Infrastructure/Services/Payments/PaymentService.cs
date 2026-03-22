using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Events;
using mtkpm.Domain.Enums.Business;

namespace mtkpm.Infrastructure.Services.Payments
{
    /// <summary>
    /// Payment Service - Orchestrate payment processing
    /// S? d?ng PaymentFactory ?? t?o payment methods
    /// </summary>
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentFactory _paymentFactory;
        private readonly ILoggerService _logger;

        public PaymentService(IPaymentFactory paymentFactory, ILoggerService logger)
        {
            _paymentFactory = paymentFactory;
            _logger = logger;
        }

        public async Task<PaymentResult> ProcessPaymentAsync(
            PaymentMethodType paymentType,
            decimal amount,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"Processing payment with method: {paymentType}, Amount: {amount:C}", "PaymentService");

            try
            {
                // Factory t?o payment method d?a theo type
                var paymentMethod = _paymentFactory.CreatePaymentMethod(paymentType);

                // Validate payment information
                var isValid = await paymentMethod.ValidateAsync(cancellationToken);
                if (!isValid)
                {
                    return new PaymentResult
                    {
                        Success = false,
                        Message = $"{paymentType} validation failed",
                        TransactionDate = DateTime.UtcNow,
                        Status = PaymentStatus.Failed
                    };
                }

                // Process payment
                var result = await paymentMethod.ProcessPaymentAsync(amount, cancellationToken);

                _logger.LogInfo($"Payment processed: {result.TransactionId}, Status: {result.Status}", "PaymentService");
                return result;
            }
            catch (NotSupportedException ex)
            {
                _logger.LogError($"Payment method not supported: {ex.Message}", "PaymentService");
                return new PaymentResult
                {
                    Success = false,
                    Message = ex.Message,
                    TransactionDate = DateTime.UtcNow,
                    Status = PaymentStatus.Failed
                };
            }
        }

        public async Task<bool> RefundPaymentAsync(
            PaymentMethodType paymentType,
            string transactionId,
            decimal amount,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"Refunding payment: {transactionId}, Method: {paymentType}, Amount: {amount:C}", "PaymentService");

            try
            {
                var paymentMethod = _paymentFactory.CreatePaymentMethod(paymentType);
                var result = await paymentMethod.RefundAsync(transactionId, amount, cancellationToken);

                _logger.LogInfo($"Refund completed: {transactionId}, Success: {result}", "PaymentService");
                return result;
            }
            catch (NotSupportedException ex)
            {
                _logger.LogError($"Refund failed - unsupported method: {ex.Message}", "PaymentService");
                return false;
            }
        }

        public async Task<PaymentStatus> GetPaymentStatusAsync(
            PaymentMethodType paymentType,
            string transactionId,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"Checking payment status: {transactionId}, Method: {paymentType}", "PaymentService");

            try
            {
                var paymentMethod = _paymentFactory.CreatePaymentMethod(paymentType);
                var status = await paymentMethod.CheckPaymentStatusAsync(transactionId, cancellationToken);

                _logger.LogInfo($"Payment status: {transactionId} - {status}", "PaymentService");
                return status;
            }
            catch (NotSupportedException ex)
            {
                _logger.LogError($"Status check failed - unsupported method: {ex.Message}", "PaymentService");
                return PaymentStatus.Failed;
            }
        }
    }
}
