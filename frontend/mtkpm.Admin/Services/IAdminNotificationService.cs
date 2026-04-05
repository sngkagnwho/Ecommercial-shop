using System.Text.Json;
using mtkpm.Admin.Models.Notification;

namespace mtkpm.Admin.Services
{
    /// <summary>
    /// Admin service for managing notification methods and testing notification system
    /// </summary>
    public interface IAdminNotificationService
    {
        Task<List<NotificationMethodViewModel>?> GetNotificationMethodsAsync();
        Task<bool> SubscribeNotificationMethodAsync(string methodName, NotificationSubscriptionRequest request);
        Task<bool> UnsubscribeNotificationMethodAsync(string methodName);
        Task<List<NotificationSubscriberViewModel>?> GetSubscribersAsync();
        Task<NotificationTestResultViewModel?> TestOrderCreatedEventAsync();
        Task<NotificationTestResultViewModel?> TestPaymentCompletedEventAsync();
        Task<NotificationTestResultViewModel?> TestOrderShippedEventAsync();
        Task<NotificationTestResultViewModel?> TestPaymentFailedEventAsync();
        Task<NotificationTestResultViewModel?> TestOrderCancelledEventAsync();
    }

    /// <summary>
    /// Implementation of notification admin service
    /// </summary>
    public class AdminNotificationService : IAdminNotificationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenManager _tokenManager;
        private readonly ILogger<AdminNotificationService> _logger;
        private readonly IConfiguration _configuration;

        public AdminNotificationService(
            IHttpClientFactory httpClientFactory,
            ITokenManager tokenManager,
            ILogger<AdminNotificationService> logger,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _tokenManager = tokenManager;
            _logger = logger;
            _configuration = configuration;
        }

        private string GetApiBaseUrl()
        {
            return _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:5107";
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
        /// Get all available notification methods
        /// GET /api/notification/methods
        /// </summary>
        public async Task<List<NotificationMethodViewModel>?> GetNotificationMethodsAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                SetAuthHeader(client);

                var apiUrl = GetApiBaseUrl();
                var response = await client.GetAsync($"{apiUrl}/api/notification/methods");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var methods = JsonSerializer.Deserialize<List<NotificationMethodViewModel>>(content, options);
                    return methods;
                }

                _logger.LogWarning($"Failed to get notification methods: {response.StatusCode}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting notification methods: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Subscribe to a notification method
        /// POST /api/notification/methods/{methodName}/subscribe
        /// </summary>
        public async Task<bool> SubscribeNotificationMethodAsync(string methodName, NotificationSubscriptionRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                SetAuthHeader(client);

                var apiUrl = GetApiBaseUrl();
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{apiUrl}/api/notification/methods/{methodName}/subscribe", content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Successfully subscribed to notification method: {methodName}");
                    return true;
                }

                _logger.LogWarning($"Failed to subscribe to notification method {methodName}: {response.StatusCode}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error subscribing to notification method: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Unsubscribe from a notification method
        /// DELETE /api/notification/methods/{methodName}
        /// </summary>
        public async Task<bool> UnsubscribeNotificationMethodAsync(string methodName)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                SetAuthHeader(client);

                var apiUrl = GetApiBaseUrl();
                var response = await client.DeleteAsync($"{apiUrl}/api/notification/methods/{methodName}");

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Successfully unsubscribed from notification method: {methodName}");
                    return true;
                }

                _logger.LogWarning($"Failed to unsubscribe from notification method {methodName}: {response.StatusCode}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error unsubscribing from notification method: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Get all notification subscribers
        /// GET /api/notification/subscribers
        /// </summary>
        public async Task<List<NotificationSubscriberViewModel>?> GetSubscribersAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                SetAuthHeader(client);

                var apiUrl = GetApiBaseUrl();
                var response = await client.GetAsync($"{apiUrl}/api/notification/subscribers");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var subscribers = JsonSerializer.Deserialize<List<NotificationSubscriberViewModel>>(content, options);
                    return subscribers;
                }

                _logger.LogWarning($"Failed to get subscribers: {response.StatusCode}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting subscribers: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Test OrderCreated event notification
        /// POST /api/notification/test/OrderCreated
        /// </summary>
        public async Task<NotificationTestResultViewModel?> TestOrderCreatedEventAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                SetAuthHeader(client);

                var apiUrl = GetApiBaseUrl();
                var response = await client.PostAsync($"{apiUrl}/api/notification/test/OrderCreated", null);

                return await ParseTestResponse(response, "OrderCreated");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error testing OrderCreated event: {ex.Message}");
                return new NotificationTestResultViewModel
                {
                    EventType = "OrderCreated",
                    Success = false,
                    Message = ex.Message,
                    Timestamp = DateTime.UtcNow
                };
            }
        }

        /// <summary>
        /// Test PaymentCompleted event notification
        /// POST /api/notification/test/PaymentCompleted
        /// </summary>
        public async Task<NotificationTestResultViewModel?> TestPaymentCompletedEventAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                SetAuthHeader(client);

                var apiUrl = GetApiBaseUrl();
                var response = await client.PostAsync($"{apiUrl}/api/notification/test/PaymentCompleted", null);

                return await ParseTestResponse(response, "PaymentCompleted");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error testing PaymentCompleted event: {ex.Message}");
                return new NotificationTestResultViewModel
                {
                    EventType = "PaymentCompleted",
                    Success = false,
                    Message = ex.Message,
                    Timestamp = DateTime.UtcNow
                };
            }
        }

        /// <summary>
        /// Test OrderShipped event notification
        /// POST /api/notification/test/OrderShipped
        /// </summary>
        public async Task<NotificationTestResultViewModel?> TestOrderShippedEventAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                SetAuthHeader(client);

                var apiUrl = GetApiBaseUrl();
                var response = await client.PostAsync($"{apiUrl}/api/notification/test/OrderShipped", null);

                return await ParseTestResponse(response, "OrderShipped");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error testing OrderShipped event: {ex.Message}");
                return new NotificationTestResultViewModel
                {
                    EventType = "OrderShipped",
                    Success = false,
                    Message = ex.Message,
                    Timestamp = DateTime.UtcNow
                };
            }
        }

        /// <summary>
        /// Test PaymentFailed event notification
        /// POST /api/notification/test/PaymentFailed
        /// </summary>
        public async Task<NotificationTestResultViewModel?> TestPaymentFailedEventAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                SetAuthHeader(client);

                var apiUrl = GetApiBaseUrl();
                var response = await client.PostAsync($"{apiUrl}/api/notification/test/PaymentFailed", null);

                return await ParseTestResponse(response, "PaymentFailed");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error testing PaymentFailed event: {ex.Message}");
                return new NotificationTestResultViewModel
                {
                    EventType = "PaymentFailed",
                    Success = false,
                    Message = ex.Message,
                    Timestamp = DateTime.UtcNow
                };
            }
        }

        /// <summary>
        /// Test OrderCancelled event notification
        /// POST /api/notification/test/OrderCancelled
        /// </summary>
        public async Task<NotificationTestResultViewModel?> TestOrderCancelledEventAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                SetAuthHeader(client);

                var apiUrl = GetApiBaseUrl();
                var response = await client.PostAsync($"{apiUrl}/api/notification/test/OrderCancelled", null);

                return await ParseTestResponse(response, "OrderCancelled");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error testing OrderCancelled event: {ex.Message}");
                return new NotificationTestResultViewModel
                {
                    EventType = "OrderCancelled",
                    Success = false,
                    Message = ex.Message,
                    Timestamp = DateTime.UtcNow
                };
            }
        }

        /// <summary>
        /// Get notification implementation guide
        /// GET /api/notification/guide
        /// </summary>


        /// <summary>
        /// Helper method to parse test response
        /// </summary>
        private async Task<NotificationTestResultViewModel?> ParseTestResponse(HttpResponseMessage response, string eventType)
        {
            try
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<NotificationTestResultViewModel>(content, options);

                if (result != null)
                {
                    result.EventType = eventType;
                    if (result.Timestamp == default)
                        result.Timestamp = DateTime.UtcNow;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error parsing test response: {ex.Message}");
                return new NotificationTestResultViewModel
                {
                    EventType = eventType,
                    Success = false,
                    Message = "Error parsing response",
                    Timestamp = DateTime.UtcNow
                };
            }
        }
    }
}
