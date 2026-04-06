using System.Text.Json;
using mtkpm.Admin.Models;

namespace mtkpm.Admin.Services
{
    /// <summary>
    /// Admin Orders Management Service
    /// Handles all order-related operations from backend API
    /// </summary>
    public interface IAdminOrderService
    {
        /// <summary>
        /// Get all orders (admin only)
        /// GET /api/orders
        /// </summary>
        Task<List<OrderViewModel>> GetAllOrdersAsync();

        /// <summary>
        /// Get order by ID
        /// GET /api/orders/{id}
        /// </summary>
        Task<OrderViewModel> GetOrderByIdAsync(int id);

        /// <summary>
        /// Get order by order number
        /// GET /api/orders/number/{orderNumber}
        /// </summary>
        Task<OrderViewModel> GetOrderByNumberAsync(string orderNumber);

        /// <summary>
        /// Get orders statistics for dashboard
        /// </summary>
        Task<OrderStatisticsViewModel> GetOrderStatisticsAsync();

        /// <summary>
        /// Update order status (admin only)
        /// PATCH /api/orders/{id}/status
        /// </summary>
        Task<OrderViewModel> UpdateOrderStatusAsync(int id, int status);

        /// <summary>
        /// Mark order as paid (admin only)
        /// POST /api/orders/{id}/mark-paid
        /// </summary>
        Task<bool> MarkOrderAsPaidAsync(int id);

        /// <summary>
        /// Cancel order
        /// POST /api/orders/{id}/cancel
        /// </summary>
        Task<bool> CancelOrderAsync(int id);
    }

    /// <summary>
    /// Order view model for display
    /// </summary>
    public class OrderViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public int Status { get; set; }
        public string StatusDisplay { get; set; }
        public int PaymentMethod { get; set; }
        public string PaymentMethodDisplay { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaidAt { get; set; }
        public string Note { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string StatusBadge => Status switch
        {
            1 => "badge-warning",     // Pending
            2 => "badge-info",        // Confirmed
            3 => "badge-primary",     // Processing
            4 => "badge-secondary",   // Shipping
            5 => "badge-success",     // Delivered
            6 => "badge-success",     // Completed
            7 => "badge-danger",      // Cancelled
            8 => "badge-orange",      // Returned
            9 => "badge-dark",        // Failed
            _ => "badge-secondary"
        };

        public string PaymentBadge => IsPaid ? "badge-success" : "badge-warning";
    }

    /// <summary>
    /// Order item in order
    /// </summary>
    public class OrderItemViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtOrder { get; set; }
        public decimal TotalPrice { get; set; }
    }

    /// <summary>
    /// Orders statistics for dashboard
    /// </summary>
    public class OrderStatisticsViewModel
    {
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int ShippingOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int CancelledOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TodayRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
    }

    /// <summary>
    /// Update order status request
    /// </summary>
    public class UpdateOrderStatusRequest
    {
        public int Status { get; set; }
    }

    /// <summary>
    /// Implementation of Admin Order Service
    /// </summary>
    public class AdminOrderService : IAdminOrderService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenManager _tokenManager;
        private readonly ILogger<AdminOrderService> _logger;
        private readonly IConfiguration _configuration;

        public AdminOrderService(
            IHttpClientFactory httpClientFactory,
            ITokenManager tokenManager,
            ILogger<AdminOrderService> logger,
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

            if (baseUrl.EndsWith("/api", StringComparison.OrdinalIgnoreCase) &&
                normalizedPath.StartsWith("/api", StringComparison.OrdinalIgnoreCase))
            {
                return $"{baseUrl}{normalizedPath[4..]}";
            }

            return $"{baseUrl}{normalizedPath}";
        }

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
        };

        private void SetAuthHeader(HttpClient httpClient)
        {
            var token = _tokenManager.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                _logger.LogInformation($"✅ JWT token set - Token length: {token.Length}");
            }
            else
            {
                _logger.LogWarning("⚠️ JWT token is NULL - User may not be authenticated!");
            }
        }

        /// <summary>
        /// GET /api/orders
        /// </summary>
        public async Task<List<OrderViewModel>> GetAllOrdersAsync()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                SetAuthHeader(httpClient);

                var endpoint = BuildApiEndpoint("/api/orders");
                _logger.LogInformation($"Fetching all orders from: {endpoint}");

                var response = await httpClient.GetAsync(endpoint);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _logger.LogWarning("Unauthorized when fetching orders. Token may be expired or invalid.");
                    throw new UnauthorizedAccessException("Session expired or unauthorized access.");
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Failed to get orders: {response.StatusCode}");
                    return new List<OrderViewModel>();
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<OrderViewModel>>>(json, JsonOptions);

                return apiResponse?.Data ?? new List<OrderViewModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching all orders: {ex.Message}");
                return new List<OrderViewModel>();
            }
        }

        /// <summary>
        /// GET /api/orders/{id}
        /// </summary>
        public async Task<OrderViewModel> GetOrderByIdAsync(int id)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                SetAuthHeader(httpClient);

                var endpoint = BuildApiEndpoint($"/api/orders/{id}");
                _logger.LogInformation($"Fetching order {id}");

                var response = await httpClient.GetAsync(endpoint);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _logger.LogWarning($"Unauthorized when fetching order {id}. Token may be expired or invalid.");
                    throw new UnauthorizedAccessException("Session expired or unauthorized access.");
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Failed to get order {id}: {response.StatusCode}");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<OrderViewModel>>(json, JsonOptions);

                return apiResponse?.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching order {id}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// GET /api/orders/number/{orderNumber}
        /// </summary>
        public async Task<OrderViewModel> GetOrderByNumberAsync(string orderNumber)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                SetAuthHeader(httpClient);

                var endpoint = BuildApiEndpoint($"/api/orders/number/{orderNumber}");
                _logger.LogInformation($"Fetching order by number: {orderNumber}");

                var response = await httpClient.GetAsync(endpoint);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Failed to get order by number: {response.StatusCode}");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<OrderViewModel>>(json, JsonOptions);

                return apiResponse?.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching order by number: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Calculate order statistics
        /// </summary>
        public async Task<OrderStatisticsViewModel> GetOrderStatisticsAsync()
        {
            try
            {
                var orders = await GetAllOrdersAsync();
                var stats = new OrderStatisticsViewModel
                {
                    TotalOrders = orders.Count,
                    PendingOrders = orders.Count(o => o.Status == 1),
                    ShippingOrders = orders.Count(o => o.Status == 4),
                    CompletedOrders = orders.Count(o => o.Status == 6),
                    CancelledOrders = orders.Count(o => o.Status == 7),
                    TotalRevenue = orders.Sum(o => o.TotalAmount),
                    TodayRevenue = orders.Where(o => o.CreatedAt.Date == DateTime.Today).Sum(o => o.TotalAmount),
                    AverageOrderValue = orders.Count > 0 ? orders.Average(o => o.TotalAmount) : 0
                };

                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calculating order statistics: {ex.Message}");
                return new OrderStatisticsViewModel();
            }
        }

        /// <summary>
        /// PATCH /api/orders/{id}/status
        /// </summary>
        public async Task<OrderViewModel> UpdateOrderStatusAsync(int id, int status)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                SetAuthHeader(httpClient);

                var endpoint = BuildApiEndpoint($"/api/orders/{id}/status");
                var request = new UpdateOrderStatusRequest { Status = status };

                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(request),
                    System.Text.Encoding.UTF8,
                    "application/json");

                _logger.LogInformation($"Updating order {id} status to {status}");
                var response = await httpClient.PatchAsync(endpoint, jsonContent);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _logger.LogWarning($"Unauthorized when updating order {id} status.");
                    throw new UnauthorizedAccessException("Session expired or unauthorized access.");
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Failed to update order status: {response.StatusCode}");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<bool>>(json, JsonOptions);

                if (apiResponse?.Data == true)
                {
                    return await GetOrderByIdAsync(id);
                }

                _logger.LogWarning($"Order {id} status update API returned unsuccessful payload.");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating order status: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// POST /api/orders/{id}/mark-paid
        /// </summary>
        public async Task<bool> MarkOrderAsPaidAsync(int id)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                SetAuthHeader(httpClient);

                var endpoint = BuildApiEndpoint($"/api/orders/{id}/mark-paid");
                _logger.LogInformation($"Marking order {id} as paid");

                var response = await httpClient.PostAsync(endpoint, new StringContent("", System.Text.Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Failed to mark order as paid: {response.StatusCode}");
                    return false;
                }

                _logger.LogInformation($"✅ Order {id} marked as paid");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error marking order as paid: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// POST /api/orders/{id}/cancel
        /// </summary>
        public async Task<bool> CancelOrderAsync(int id)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                SetAuthHeader(httpClient);

                var endpoint = BuildApiEndpoint($"/api/orders/{id}/cancel");
                _logger.LogInformation($"Cancelling order {id}");

                var response = await httpClient.PostAsync(endpoint, new StringContent("", System.Text.Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Failed to cancel order: {response.StatusCode}");
                    return false;
                }

                _logger.LogInformation($"✅ Order {id} cancelled");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error cancelling order: {ex.Message}");
                throw;
            }
        }
    }
}
