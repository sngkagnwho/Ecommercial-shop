using mtkpm.Admin.Features.Orders.Models;
using mtkpm.Admin.Services;

namespace mtkpm.Admin.Features.Orders.Services
{
    /// <summary>
    /// Interface for managing user addresses in Orders context
    /// </summary>
    public interface IUserAddressService
    {
        /// <summary>
        /// Get all saved addresses for current user
        /// </summary>
        Task<List<UserAddressViewModel>> GetMyAddressesAsync();

        /// <summary>
        /// Get specific address by ID
        /// </summary>
        Task<UserAddressViewModel?> GetAddressAsync(int addressId);

        /// <summary>
        /// Create new address
        /// </summary>
        Task<UserAddressViewModel?> CreateAddressAsync(CreateUserAddressViewModel model);

        /// <summary>
        /// Update existing address
        /// </summary>
        Task<UserAddressViewModel?> UpdateAddressAsync(int addressId, UpdateUserAddressViewModel model);

        /// <summary>
        /// Delete address
        /// </summary>
        Task<bool> DeleteAddressAsync(int addressId);
    }

    /// <summary>
    /// Implementation of user address service
    /// </summary>
    public class UserAddressService : IUserAddressService
    {
        private readonly ITokenManager _tokenManager;
        private readonly ILogger<UserAddressService> _logger;
        private readonly IConfiguration _configuration;

        public UserAddressService(
            ITokenManager tokenManager,
            ILogger<UserAddressService> logger,
            IConfiguration configuration)
        {
            _tokenManager = tokenManager;
            _logger = logger;
            _configuration = configuration;
        }

        private string GetApiBaseUrl()
        {
            return _configuration["Backend:ApiUrl"] ?? "https://localhost:5107/api";
        }

        private HttpClient GetHttpClientWithAuth()
        {
            var httpClient = new HttpClient();
            var token = _tokenManager.GetToken();
            
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            
            return httpClient;
        }

        public async Task<List<UserAddressViewModel>> GetMyAddressesAsync()
        {
            try
            {
                var httpClient = GetHttpClientWithAuth();
                var apiUrl = GetApiBaseUrl();
                
                var response = await httpClient.GetAsync($"{apiUrl}/useraddresses");
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Failed to fetch addresses: {response.StatusCode}");
                    return new List<UserAddressViewModel>();
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                
                var addresses = new List<UserAddressViewModel>();
                
                using (System.Text.Json.JsonDocument doc = System.Text.Json.JsonDocument.Parse(responseContent))
                {
                    var root = doc.RootElement;
                    if (root.TryGetProperty("data", out var dataElement) && 
                        dataElement.ValueKind == System.Text.Json.JsonValueKind.Array)
                    {
                        var itemsJson = dataElement.GetRawText();
                        addresses = System.Text.Json.JsonSerializer.Deserialize<List<UserAddressViewModel>>(itemsJson, options) 
                            ?? new List<UserAddressViewModel>();
                    }
                }

                _logger.LogInformation($"Loaded {addresses.Count} addresses");
                return addresses;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching addresses: {ex.Message}");
                return new List<UserAddressViewModel>();
            }
        }

        public async Task<UserAddressViewModel?> GetAddressAsync(int addressId)
        {
            try
            {
                var httpClient = GetHttpClientWithAuth();
                var apiUrl = GetApiBaseUrl();
                
                var response = await httpClient.GetAsync($"{apiUrl}/useraddresses/{addressId}");
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Failed to fetch address {addressId}: {response.StatusCode}");
                    return null;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                
                UserAddressViewModel? address = null;
                
                using (System.Text.Json.JsonDocument doc = System.Text.Json.JsonDocument.Parse(responseContent))
                {
                    var root = doc.RootElement;
                    if (root.TryGetProperty("data", out var dataElement) && 
                        dataElement.ValueKind != System.Text.Json.JsonValueKind.Null)
                    {
                        address = System.Text.Json.JsonSerializer.Deserialize<UserAddressViewModel>(
                            dataElement.GetRawText(), options);
                    }
                }

                return address;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching address: {ex.Message}");
                return null;
            }
        }

        public async Task<UserAddressViewModel?> CreateAddressAsync(CreateUserAddressViewModel model)
        {
            try
            {
                var httpClient = GetHttpClientWithAuth();
                var apiUrl = GetApiBaseUrl();
                
                var content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(model),
                    System.Text.Encoding.UTF8,
                    "application/json");

                var response = await httpClient.PostAsync($"{apiUrl}/useraddresses", content);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning($"Failed to create address: {response.StatusCode} - {errorContent}");
                    return null;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                
                UserAddressViewModel? address = null;
                
                using (System.Text.Json.JsonDocument doc = System.Text.Json.JsonDocument.Parse(responseContent))
                {
                    var root = doc.RootElement;
                    if (root.TryGetProperty("data", out var dataElement) && 
                        dataElement.ValueKind != System.Text.Json.JsonValueKind.Null)
                    {
                        address = System.Text.Json.JsonSerializer.Deserialize<UserAddressViewModel>(
                            dataElement.GetRawText(), options);
                    }
                }

                _logger.LogInformation($"Address created: {address?.Id}");
                return address;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating address: {ex.Message}");
                return null;
            }
        }

        public async Task<UserAddressViewModel?> UpdateAddressAsync(int addressId, UpdateUserAddressViewModel model)
        {
            try
            {
                var httpClient = GetHttpClientWithAuth();
                var apiUrl = GetApiBaseUrl();
                
                var content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(model),
                    System.Text.Encoding.UTF8,
                    "application/json");

                var response = await httpClient.PutAsync($"{apiUrl}/useraddresses/{addressId}", content);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning($"Failed to update address: {response.StatusCode} - {errorContent}");
                    return null;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                
                UserAddressViewModel? address = null;
                
                using (System.Text.Json.JsonDocument doc = System.Text.Json.JsonDocument.Parse(responseContent))
                {
                    var root = doc.RootElement;
                    if (root.TryGetProperty("data", out var dataElement) && 
                        dataElement.ValueKind != System.Text.Json.JsonValueKind.Null)
                    {
                        address = System.Text.Json.JsonSerializer.Deserialize<UserAddressViewModel>(
                            dataElement.GetRawText(), options);
                    }
                }

                _logger.LogInformation($"Address updated: {addressId}");
                return address;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating address: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteAddressAsync(int addressId)
        {
            try
            {
                var httpClient = GetHttpClientWithAuth();
                var apiUrl = GetApiBaseUrl();
                
                var response = await httpClient.DeleteAsync($"{apiUrl}/useraddresses/{addressId}");
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Failed to delete address: {response.StatusCode}");
                    return false;
                }

                _logger.LogInformation($"Address deleted: {addressId}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting address: {ex.Message}");
                return false;
            }
        }
    }
}
