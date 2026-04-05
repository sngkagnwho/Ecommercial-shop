using System.Text.Json;
using mtkpm.Admin.Models.Discount;

namespace mtkpm.Admin.Services
{
    /// <summary>
    /// Admin service for discount code management and testing
    /// Supports both CRUD operations and discount calculator with stacking
    /// </summary>
    public interface IAdminDiscountService
    {
        // Public endpoints
        Task<List<DiscountCodeDto>?> GetAvailableDiscountsAsync();
        Task<CalculateDiscountResponse?> CalculateDiscountAsync(List<string> discountCodes);
        
        // Admin CRUD endpoints
        Task<List<DiscountViewModel>?> GetDiscountsAsync(bool includeInactive = true);
        Task<DiscountViewModel?> GetDiscountByIdAsync(int id);
        Task<DiscountViewModel?> CreateDiscountAsync(CreateDiscountViewModel model);
        Task<DiscountViewModel?> UpdateDiscountAsync(int id, UpdateDiscountViewModel model);
        Task<bool> DeleteDiscountAsync(int id);
        
        // Statistics
        Task<DiscountStatisticsViewModel?> GetDiscountStatisticsAsync();
    }

    /// <summary>
    /// Implementation of admin discount service
    /// </summary>
    public class AdminDiscountService : IAdminDiscountService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenManager _tokenManager;
        private readonly ILogger<AdminDiscountService> _logger;
        private readonly IConfiguration _configuration;

        public AdminDiscountService(
            IHttpClientFactory httpClientFactory,
            ITokenManager tokenManager,
            ILogger<AdminDiscountService> logger,
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
        /// Get all available discount codes
        /// GET /api/discount/available
        /// </summary>
        public async Task<List<DiscountCodeDto>?> GetAvailableDiscountsAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                // Don't require auth for available discounts
                
                var apiUrl = GetApiBaseUrl();
                var response = await client.GetAsync($"{apiUrl}/api/discount/available");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    
                    // Parse ApiResponse wrapper
                    using var doc = JsonDocument.Parse(content);
                    var root = doc.RootElement;
                    
                    if (root.TryGetProperty("data", out var dataElement))
                    {
                        var discounts = JsonSerializer.Deserialize<List<DiscountCodeDto>>(dataElement.GetRawText(), options);
                        return discounts;
                    }
                }

                _logger.LogWarning($"Failed to get available discounts: {response.StatusCode}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting available discounts: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Calculate discount for cart
        /// POST /api/discount/calculate
        /// </summary>
        public async Task<CalculateDiscountResponse?> CalculateDiscountAsync(List<string> discountCodes)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                SetAuthHeader(client);

                var apiUrl = GetApiBaseUrl();
                var request = new { discountCodes };
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{apiUrl}/api/discount/calculate", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    
                    // Parse ApiResponse wrapper
                    using var doc = JsonDocument.Parse(responseContent);
                    var root = doc.RootElement;
                    
                    if (root.TryGetProperty("data", out var dataElement))
                    {
                        var result = JsonSerializer.Deserialize<CalculateDiscountResponse>(dataElement.GetRawText(), options);
                        return result;
                    }
                }

                _logger.LogWarning($"Failed to calculate discount: {response.StatusCode}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calculating discount: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Get all discounts (admin only)
        /// GET /api/discount?includeInactive=true/false
        /// </summary>
        public async Task<List<DiscountViewModel>?> GetDiscountsAsync(bool includeInactive = true)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                SetAuthHeader(client);

                var apiUrl = GetApiBaseUrl();
                var response = await client.GetAsync($"{apiUrl}/api/discount?includeInactive={includeInactive}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    
                    using var doc = JsonDocument.Parse(content);
                    var root = doc.RootElement;
                    
                    if (root.TryGetProperty("data", out var dataElement))
                    {
                        var discounts = JsonSerializer.Deserialize<List<DiscountViewModel>>(dataElement.GetRawText(), options);
                        return discounts;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting discounts: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Get discount by id (admin only)
        /// GET /api/discount/{id}
        /// </summary>
        public async Task<DiscountViewModel?> GetDiscountByIdAsync(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                SetAuthHeader(client);

                var apiUrl = GetApiBaseUrl();
                var response = await client.GetAsync($"{apiUrl}/api/discount/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    
                    using var doc = JsonDocument.Parse(content);
                    var root = doc.RootElement;
                    
                    if (root.TryGetProperty("data", out var dataElement))
                    {
                        var discount = JsonSerializer.Deserialize<DiscountViewModel>(dataElement.GetRawText(), options);
                        return discount;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting discount {id}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Create discount (admin only)
        /// POST /api/discount
        /// </summary>
        public async Task<DiscountViewModel?> CreateDiscountAsync(CreateDiscountViewModel model)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                SetAuthHeader(client);

                var apiUrl = GetApiBaseUrl();
                var json = JsonSerializer.Serialize(model);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{apiUrl}/api/discount", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    
                    using var doc = JsonDocument.Parse(responseContent);
                    var root = doc.RootElement;
                    
                    if (root.TryGetProperty("data", out var dataElement))
                    {
                        var discount = JsonSerializer.Deserialize<DiscountViewModel>(dataElement.GetRawText(), options);
                        return discount;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating discount: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Update discount (admin only)
        /// PUT /api/discount/{id}
        /// </summary>
        public async Task<DiscountViewModel?> UpdateDiscountAsync(int id, UpdateDiscountViewModel model)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                SetAuthHeader(client);

                var apiUrl = GetApiBaseUrl();
                var json = JsonSerializer.Serialize(model);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"{apiUrl}/api/discount/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    
                    using var doc = JsonDocument.Parse(responseContent);
                    var root = doc.RootElement;
                    
                    if (root.TryGetProperty("data", out var dataElement))
                    {
                        var discount = JsonSerializer.Deserialize<DiscountViewModel>(dataElement.GetRawText(), options);
                        return discount;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating discount {id}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Delete discount (admin only)
        /// DELETE /api/discount/{id}
        /// </summary>
        public async Task<bool> DeleteDiscountAsync(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                SetAuthHeader(client);

                var apiUrl = GetApiBaseUrl();
                var response = await client.DeleteAsync($"{apiUrl}/api/discount/{id}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting discount {id}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Get discount statistics (admin only)
        /// </summary>
        public async Task<DiscountStatisticsViewModel?> GetDiscountStatisticsAsync()
        {
            try
            {
                var discounts = await GetDiscountsAsync(includeInactive: true);
                
                if (discounts == null)
                    return null;

                var stats = new DiscountStatisticsViewModel
                {
                    TotalDiscounts = discounts.Count,
                    ActiveDiscounts = discounts.Count(d => d.IsActive && !d.IsExpired),
                    ExpiredDiscounts = discounts.Count(d => d.IsExpired),
                    BudgetExhaustedDiscounts = discounts.Count(d => d.IsBudgetExhausted),
                    TotalBudgetLimit = discounts.Where(d => d.BudgetLimit.HasValue).Sum(d => d.BudgetLimit.Value),
                    TotalBudgetUsed = discounts.Sum(d => d.BudgetUsed),
                    TotalUsageCount = discounts.Sum(d => d.UsedCount)
                };

                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting discount statistics: {ex.Message}");
                return null;
            }
        }
    }
}
