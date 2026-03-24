using mtkpm.Admin.Constants;
using mtkpm.Admin.Models.Payment;

namespace mtkpm.Admin.Services
{
    /// <summary>
    /// Interface for payment management service
    /// </summary>
    public interface IPaymentService
    {
        Task<PaymentHistoryViewModel?> GetPaymentHistoryAsync();
        Task<PaymentViewModel?> GetPaymentByIdAsync(int id);
        Task<List<PaymentViewModel>?> GetPaymentsByOrderAsync(int orderId);
        Task<PaymentViewModel?> ProcessPaymentAsync(ProcessPaymentViewModel request);
        Task<bool> RefundPaymentAsync(int paymentId, decimal refundAmount, string reason);
    }

    /// <summary>
    /// Implementation of payment service
    /// </summary>
    public class PaymentService : IPaymentService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(IApiService apiService, ILogger<PaymentService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<PaymentHistoryViewModel?> GetPaymentHistoryAsync()
        {
            try
            {
                return await _apiService.GetAsync<PaymentHistoryViewModel>(ApiEndpoints.Payments.GetHistory);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting payment history: {ex.Message}");
                return null;
            }
        }

        public async Task<PaymentViewModel?> GetPaymentByIdAsync(int id)
        {
            try
            {
                return await _apiService.GetAsync<PaymentViewModel>($"/payments/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting payment {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<List<PaymentViewModel>?> GetPaymentsByOrderAsync(int orderId)
        {
            try
            {
                return await _apiService.GetAsync<List<PaymentViewModel>>($"/payments/order/{orderId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting payments for order {orderId}: {ex.Message}");
                return null;
            }
        }

        public async Task<PaymentViewModel?> ProcessPaymentAsync(ProcessPaymentViewModel request)
        {
            try
            {
                return await _apiService.PostAsync<PaymentViewModel>(ApiEndpoints.Payments.Process, request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing payment: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> RefundPaymentAsync(int paymentId, decimal refundAmount, string reason)
        {
            try
            {
                var request = new { refundAmount, reason };
                var result = await _apiService.PostAsync<object>($"/payments/{paymentId}/refund", request);
                return result != null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error refunding payment {paymentId}: {ex.Message}");
                return false;
            }
        }
    }
}
