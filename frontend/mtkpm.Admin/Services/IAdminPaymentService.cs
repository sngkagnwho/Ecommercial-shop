using System.Text.Json;
using mtkpm.Admin.Models;
using mtkpm.Admin.Models.Payment;

namespace mtkpm.Admin.Services
{
    /// <summary>
    /// Admin Payment Methods Management Service
    /// All methods require JWT Bearer token with Admin role
    /// </summary>
    public interface IAdminPaymentService
    {
        /// <summary>
        /// Get all payment methods
        /// </summary>
        Task<List<PaymentMethodViewModel>> GetPaymentMethodsAsync();

        /// <summary>
        /// Get payment method by code
        /// </summary>
        Task<PaymentMethodViewModel> GetPaymentMethodByCodeAsync(string code);

        /// <summary>
        /// Create new payment method (admin only)
        /// </summary>
        Task<PaymentMethodViewModel> CreatePaymentMethodAsync(CreatePaymentMethodViewModel model);

        /// <summary>
        /// Update payment method (admin only)
        /// </summary>
        Task<PaymentMethodViewModel> UpdatePaymentMethodAsync(int id, UpdatePaymentMethodViewModel model);

        /// <summary>
        /// Delete payment method (admin only)
        /// </summary>
        Task<bool> DeletePaymentMethodAsync(int id);

        /// <summary>
        /// Get payment methods statistics
        /// </summary>
        Task<PaymentStatisticsViewModel> GetPaymentStatisticsAsync();

        /// <summary>
        /// Search payment methods
        /// </summary>
        Task<List<PaymentMethodViewModel>> SearchPaymentMethodsAsync(string searchTerm);
    }

    /// <summary>
    /// Implementation of Admin Payment Service
    /// </summary>
    public class AdminPaymentService : IAdminPaymentService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenManager _tokenManager;
        private readonly ILogger<AdminPaymentService> _logger;
        private readonly IConfiguration _configuration;

        public AdminPaymentService(
            IHttpClientFactory httpClientFactory,
            ITokenManager tokenManager,
            ILogger<AdminPaymentService> logger,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _tokenManager = tokenManager;
            _logger = logger;
            _configuration = configuration;
        }

        private string GetApiBaseUrl()
        {
            return (_configuration["ApiSettings:BaseUrl"] ?? "https://localhost:5107").TrimEnd('/');
        }

        private string BuildApiEndpoint(string path)
        {
            var baseUrl = GetApiBaseUrl();
            var normalizedPath = path.StartsWith('/') ? path : $"/{path}";

            // Support both BaseUrl formats:
            // - https://localhost:5107
            // - https://localhost:5107/api
            if (baseUrl.EndsWith("/api", StringComparison.OrdinalIgnoreCase) &&
                normalizedPath.StartsWith("/api", StringComparison.OrdinalIgnoreCase))
            {
                return $"{baseUrl}{normalizedPath[4..]}";
            }

            return $"{baseUrl}{normalizedPath}";
        }

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private static string BuildBackendErrorMessage(string fallbackMessage, string? responseContent = null)
        {
            if (string.IsNullOrWhiteSpace(responseContent))
            {
                return fallbackMessage;
            }

            try
            {
                var apiError = JsonSerializer.Deserialize<ApiResponse<JsonElement>>(responseContent, JsonOptions);
                if (apiError == null)
                {
                    return fallbackMessage;
                }

                var message = !string.IsNullOrWhiteSpace(apiError.Message)
                    ? apiError.Message
                    : fallbackMessage;

                if (apiError.Errors != null && apiError.Errors.Count > 0)
                {
                    return $"{message}: {string.Join(" | ", apiError.Errors)}";
                }

                return message;
            }
            catch
            {
                return fallbackMessage;
            }
        }

        private void SetAuthHeader(HttpClient httpClient)
        {
            var token = _tokenManager.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        /// <summary>
        /// GET /api/payment/methods
        /// </summary>
        public async Task<List<PaymentMethodViewModel>> GetPaymentMethodsAsync()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                SetAuthHeader(httpClient);

                var endpoint = BuildApiEndpoint("/api/payment/methods");

                _logger.LogInformation($"Fetching payment methods from {endpoint}");
                var response = await httpClient.GetAsync(endpoint);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Failed to get payment methods: {response.StatusCode}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new InvalidOperationException(BuildBackendErrorMessage("Không th? t?i danh sách ph??ng th?c thanh toán", errorContent));
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<PaymentMethodViewModel>>>(json, JsonOptions);

                if (apiResponse?.Data == null)
                    return new List<PaymentMethodViewModel>();

                return apiResponse.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching payment methods: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// GET /api/payment/methods/{code}
        /// </summary>
        public async Task<PaymentMethodViewModel> GetPaymentMethodByCodeAsync(string code)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                SetAuthHeader(httpClient);

                var endpoint = BuildApiEndpoint($"/api/payment/methods/{code}");

                _logger.LogInformation($"Fetching payment method: {code}");
                var response = await httpClient.GetAsync(endpoint);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Payment method not found: {code}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new InvalidOperationException(BuildBackendErrorMessage("Không tìm th?y ph??ng th?c thanh toán", errorContent));
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<PaymentMethodViewModel>>(json, JsonOptions);

                return apiResponse?.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching payment method {code}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// POST /api/payment/methods
        /// </summary>
        public async Task<PaymentMethodViewModel> CreatePaymentMethodAsync(CreatePaymentMethodViewModel model)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                SetAuthHeader(httpClient);

                var endpoint = BuildApiEndpoint("/api/payment/methods");

                var jsonContent = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(model),
                    System.Text.Encoding.UTF8,
                    "application/json");

                _logger.LogInformation($"Creating payment method: {model.Code}");
                var response = await httpClient.PostAsync(endpoint, jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Failed to create payment method: {response.StatusCode}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new InvalidOperationException(BuildBackendErrorMessage("T?o ph??ng th?c thanh toán th?t b?i", errorContent));
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<PaymentMethodViewModel>>(json, JsonOptions);

                return apiResponse?.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating payment method: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// PUT /api/payment/methods/{id}
        /// </summary>
        public async Task<PaymentMethodViewModel> UpdatePaymentMethodAsync(int id, UpdatePaymentMethodViewModel model)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                SetAuthHeader(httpClient);

                var endpoint = BuildApiEndpoint($"/api/payment/methods/{id}");

                var jsonContent = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(model),
                    System.Text.Encoding.UTF8,
                    "application/json");

                _logger.LogInformation($"Updating payment method {id}");
                var response = await httpClient.PutAsync(endpoint, jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Failed to update payment method: {response.StatusCode}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new InvalidOperationException(BuildBackendErrorMessage("C?p nh?t ph??ng th?c thanh toán th?t b?i", errorContent));
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<PaymentMethodViewModel>>(json, JsonOptions);

                return apiResponse?.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating payment method {id}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// DELETE /api/payment/methods/{id}
        /// </summary>
        public async Task<bool> DeletePaymentMethodAsync(int id)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                SetAuthHeader(httpClient);

                var endpoint = BuildApiEndpoint($"/api/payment/methods/{id}");

                _logger.LogInformation($"Deleting payment method {id}");
                var response = await httpClient.DeleteAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Payment method {id} deleted successfully");
                    return true;
                }

                _logger.LogWarning($"Failed to delete payment method: {response.StatusCode}");
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException(BuildBackendErrorMessage("Xóa ph??ng th?c thanh toán th?t b?i", errorContent));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting payment method {id}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Mock statistics (no direct API endpoint)
        /// </summary>
        public async Task<PaymentStatisticsViewModel> GetPaymentStatisticsAsync()
        {
            try
            {
                var methods = await GetPaymentMethodsAsync();

                return new PaymentStatisticsViewModel
                {
                    TotalMethods = methods.Count,
                    ActiveMethods = methods.Count(m => m.IsActive),
                    InactiveMethods = methods.Count(m => !m.IsActive),
                    HighestFeePercentage = methods.Any() ? methods.Max(m => m.TransactionFeePercentage) : 0,
                    AverageFeePercentage = methods.Any() ? methods.Average(m => m.TransactionFeePercentage) : 0
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting payment statistics: {ex.Message}");
                return new PaymentStatisticsViewModel();
            }
        }

        /// <summary>
        /// Search payment methods by name/code
        /// </summary>
        public async Task<List<PaymentMethodViewModel>> SearchPaymentMethodsAsync(string searchTerm)
        {
            try
            {
                var methods = await GetPaymentMethodsAsync();

                if (string.IsNullOrWhiteSpace(searchTerm))
                    return methods;

                var term = searchTerm.ToLower();
                return methods
                    .Where(m => m.Name.ToLower().Contains(term) || 
                               m.Code.ToLower().Contains(term) ||
                               m.Description?.ToLower().Contains(term) == true)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error searching payment methods: {ex.Message}");
                return new List<PaymentMethodViewModel>();
            }
        }
    }
}
