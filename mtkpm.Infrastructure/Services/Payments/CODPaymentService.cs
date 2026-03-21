using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Enums.Business;

namespace mtkpm.Infrastructure.Services.Payments
{
    /// <summary>
    /// Cash On Delivery (COD) Payment Method Implementation
    /// </summary>
    public class CODPaymentService : IPaymentMethod
    {
        private readonly ILoggerService _logger;

        public CODPaymentService(ILoggerService logger)
        {
            _logger = logger;
        }

        public async Task<bool> ValidateAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInfo("Validating Cash On Delivery payment", "Payment");

            // COD không c?n validation
            await Task.Delay(50, cancellationToken);

            _logger.LogInfo("COD validation passed", "Payment");
            return true;
        }

        public async Task<PaymentResult> ProcessPaymentAsync(decimal amount, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"Processing COD payment: {amount:C}", "Payment");

            try
            {
                // COD không c?n x? lý ngay
                await Task.Delay(100, cancellationToken);

                var transactionId = $"COD-{Guid.NewGuid().ToString("N").Substring(0, 16).ToUpper()}";

                var result = new PaymentResult
                {
                    Success = true,
                    TransactionId = transactionId,
                    Message = "COD order confirmed. Payment will be collected on delivery.",
                    TransactionDate = DateTime.UtcNow,
                    Status = PaymentStatus.Pending // Ch? khi nh?n ???c hàng
                };

                _logger.LogInfo($"COD order confirmed: {transactionId}", "Payment");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"COD payment processing failed: {ex.Message}", "Payment");
                return new PaymentResult
                {
                    Success = false,
                    Message = ex.Message,
                    TransactionDate = DateTime.UtcNow,
                    Status = PaymentStatus.Failed
                };
            }
        }

        public async Task<bool> RefundAsync(string transactionId, decimal amount, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"Processing COD refund: {transactionId} - Amount: {amount:C}", "Payment");

            try
            {
                // Mark order as refunded
                await Task.Delay(200, cancellationToken);

                _logger.LogInfo($"COD refund marked: {transactionId}", "Payment");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"COD refund failed: {ex.Message}", "Payment");
                return false;
            }
        }

        public async Task<PaymentStatus> CheckPaymentStatusAsync(string transactionId, CancellationToken cancellationToken = default)
        {
            _logger.LogInfo($"Checking COD payment status: {transactionId}", "Payment");

            await Task.Delay(50, cancellationToken);

            // COD status depends on delivery
            return PaymentStatus.Pending;
        }
    }
}
