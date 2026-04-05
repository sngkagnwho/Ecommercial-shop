using mtkpm.Admin.Constants;
using mtkpm.Admin.Models.User;

namespace mtkpm.Admin.Services
{
    /// <summary>
    /// Interface for user management service
    /// </summary>
    public interface IUserService
    {
        // List & Search
        Task<List<UserViewModel>?> GetUsersAsync(int pageIndex, int pageSize);
        Task<List<UserViewModel>?> SearchUsersAsync(string searchTerm);
        
        // Individual User (Public)
        Task<UserViewModel?> GetUserByIdAsync(int id);
        
        // Admin User Details
        Task<UserWithRolesViewModel?> GetUserDetailForAdminAsync(int id);
        
        // Create User
        Task<UserViewModel?> CreateUserAsync(CreateUserViewModel request);
        
        // Update User
        Task<UserViewModel?> UpdateUserAsync(int id, UpdateUserViewModel request);
        
        // Delete User
        Task<bool> DeleteUserAsync(int id);
        
        // Current User
        Task<UserViewModel?> GetCurrentUserAsync();
        Task<UserViewModel?> UpdateCurrentUserAsync(UpdateCurrentUserViewModel request);
        
        // Admin Operations
        Task<bool> UpdateUserRoleAsync(int userId, string roleName);
        Task<bool> LockUserAsync(int userId, bool isLocked);
        
        // Statistics
        Task<UserStatisticsViewModel?> GetUserStatisticsAsync();
    }

    /// <summary>
    /// Implementation of user service
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IApiService _apiService;
        private readonly ITokenManager _tokenManager;
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserService(IApiService apiService, ITokenManager tokenManager, ILogger<UserService> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _apiService = apiService;
            _tokenManager = tokenManager;
            _logger = logger;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        private void SetAuthHeader()
        {
            var token = _tokenManager.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                _apiService.SetAuthorizationHeader(token);
            }
        }

        private string GetApiBaseUrl()
        {
            return _configuration["Backend:ApiUrl"] ?? "https://localhost:5107/api";
        }

        public async Task<List<UserViewModel>?> GetUsersAsync(int pageIndex, int pageSize)
        {
            try
            {
                var token = _tokenManager.GetToken();
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("No token available for GetUsersAsync");
                    return null;
                }

                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                
                var apiUrl = GetApiBaseUrl();
                var endpoint = $"{apiUrl}/users?pageIndex={pageIndex}&pageSize={pageSize}";
                
                var response = await httpClient.GetAsync(endpoint);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation($"GET users - Status: {response.StatusCode}");
                
                if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(responseContent))
                {
                    _logger.LogWarning($"API returned error: {response.StatusCode}");
                    return null;
                }

                try
                {
                    var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    
                    using (var doc = System.Text.Json.JsonDocument.Parse(responseContent))
                    {
                        var root = doc.RootElement;
                        
                        // Check if response has "data" property
                        if (root.TryGetProperty("data", out var dataElement))
                        {
                            // Check if data is an object with "items" property
                            if (dataElement.ValueKind == System.Text.Json.JsonValueKind.Object && 
                                dataElement.TryGetProperty("items", out var itemsElement))
                            {
                                var itemsJson = itemsElement.GetRawText();
                                var users = System.Text.Json.JsonSerializer.Deserialize<List<UserViewModel>>(itemsJson, options) ?? new List<UserViewModel>();
                                _logger.LogInformation($"Users loaded from paginated response: {users.Count}");
                                return users;
                            }
                            // Check if data is directly an array
                            else if (dataElement.ValueKind == System.Text.Json.JsonValueKind.Array)
                            {
                                var itemsJson = dataElement.GetRawText();
                                var users = System.Text.Json.JsonSerializer.Deserialize<List<UserViewModel>>(itemsJson, options) ?? new List<UserViewModel>();
                                _logger.LogInformation($"Users loaded from array response: {users.Count}");
                                return users;
                            }
                        }
                        else if (root.ValueKind == System.Text.Json.JsonValueKind.Array)
                        {
                            // Response is directly an array
                            var itemsJson = root.GetRawText();
                            var users = System.Text.Json.JsonSerializer.Deserialize<List<UserViewModel>>(itemsJson, options) ?? new List<UserViewModel>();
                            _logger.LogInformation($"Users loaded from direct array response: {users.Count}");
                            return users;
                        }
                    }
                }
                catch (Exception parseEx)
                {
                    _logger.LogWarning($"Error parsing API response: {parseEx.Message}");
                    return null;
                }

                _logger.LogWarning("Unable to extract users from API response");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting users: {ex.Message}");
                return null;
            }
        }

        public async Task<UserViewModel?> GetUserByIdAsync(int id)
        {
            try
            {
                var token = _tokenManager.GetToken();
                // Public endpoint - no token required
                
                var httpClient = _httpClientFactory.CreateClient();
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
                
                var apiUrl = GetApiBaseUrl();
                var endpoint = $"{apiUrl}/users/{id}";
                
                var response = await httpClient.GetAsync(endpoint);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation($"GET users/{id} - Status: {response.StatusCode}");
                
                if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(responseContent))
                {
                    _logger.LogWarning($"API returned error: {response.StatusCode}");
                    return null;
                }

                try
                {
                    var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    
                    using (var doc = System.Text.Json.JsonDocument.Parse(responseContent))
                    {
                        var root = doc.RootElement;
                        
                        if (root.TryGetProperty("data", out var dataElement) && 
                            dataElement.ValueKind == System.Text.Json.JsonValueKind.Object)
                        {
                            var itemsJson = dataElement.GetRawText();
                            var user = System.Text.Json.JsonSerializer.Deserialize<UserViewModel>(itemsJson, options);
                            _logger.LogInformation($"User loaded: {user?.Id}");
                            return user;
                        }
                    }
                }
                catch (Exception parseEx)
                {
                    _logger.LogWarning($"Error parsing API response: {parseEx.Message}");
                    return null;
                }

                _logger.LogWarning("Unable to extract user from API response");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting user {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<UserViewModel?> CreateUserAsync(CreateUserViewModel request)
        {
            try
            {
                var token = _tokenManager.GetToken();
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("No token available for CreateUserAsync");
                    return null;
                }

                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                
                var apiUrl = GetApiBaseUrl();
                var endpoint = $"{apiUrl}/users";
                
                var jsonContent = new System.Net.Http.StringContent(
                    System.Text.Json.JsonSerializer.Serialize(request),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );
                
                var response = await httpClient.PostAsync(endpoint, jsonContent);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation($"POST users - Status: {response.StatusCode}");
                
                if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(responseContent))
                {
                    _logger.LogWarning($"API returned error: {response.StatusCode}");
                    return null;
                }

                try
                {
                    var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    
                    using (var doc = System.Text.Json.JsonDocument.Parse(responseContent))
                    {
                        var root = doc.RootElement;
                        
                        if (root.TryGetProperty("data", out var dataElement) && 
                            dataElement.ValueKind == System.Text.Json.JsonValueKind.Object)
                        {
                            var itemsJson = dataElement.GetRawText();
                            var user = System.Text.Json.JsonSerializer.Deserialize<UserViewModel>(itemsJson, options);
                            _logger.LogInformation($"User created: {user?.Id}");
                            return user;
                        }
                    }
                }
                catch (Exception parseEx)
                {
                    _logger.LogWarning($"Error parsing API response: {parseEx.Message}");
                    return null;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating user: {ex.Message}");
                return null;
            }
        }

        public async Task<UserViewModel?> UpdateUserAsync(int id, UpdateUserViewModel request)
        {
            try
            {
                var token = _tokenManager.GetToken();
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("No token available for UpdateUserAsync");
                    return null;
                }

                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                
                var apiUrl = GetApiBaseUrl();
                var endpoint = $"{apiUrl}/users/{id}";
                
                var jsonContent = new System.Net.Http.StringContent(
                    System.Text.Json.JsonSerializer.Serialize(request),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );
                
                var response = await httpClient.PutAsync(endpoint, jsonContent);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation($"PUT users/{id} - Status: {response.StatusCode}");
                
                if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(responseContent))
                {
                    _logger.LogWarning($"API returned error: {response.StatusCode}");
                    return null;
                }

                try
                {
                    var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    
                    using (var doc = System.Text.Json.JsonDocument.Parse(responseContent))
                    {
                        var root = doc.RootElement;
                        
                        if (root.TryGetProperty("data", out var dataElement) && 
                            dataElement.ValueKind == System.Text.Json.JsonValueKind.Object)
                        {
                            var itemsJson = dataElement.GetRawText();
                            var user = System.Text.Json.JsonSerializer.Deserialize<UserViewModel>(itemsJson, options);
                            _logger.LogInformation($"User updated: {user?.Id}");
                            return user;
                        }
                    }
                }
                catch (Exception parseEx)
                {
                    _logger.LogWarning($"Error parsing API response: {parseEx.Message}");
                    return null;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating user {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var token = _tokenManager.GetToken();
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("No token available for DeleteUserAsync");
                    return false;
                }

                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                
                var apiUrl = GetApiBaseUrl();
                var endpoint = $"{apiUrl}/users/{id}";
                
                var response = await httpClient.DeleteAsync(endpoint);
                
                _logger.LogInformation($"DELETE users/{id} - Status: {response.StatusCode}");
                
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting user {id}: {ex.Message}");
                return false;
            }
        }

        public async Task<List<UserViewModel>?> SearchUsersAsync(string searchTerm)
        {
            try
            {
                var token = _tokenManager.GetToken();
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("No token available for SearchUsersAsync");
                    return null;
                }

                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                
                var apiUrl = GetApiBaseUrl();
                var endpoint = $"{apiUrl}/users/search?searchTerm={Uri.EscapeDataString(searchTerm)}";
                
                var response = await httpClient.GetAsync(endpoint);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation($"GET users/search - Status: {response.StatusCode}");
                
                if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(responseContent))
                {
                    _logger.LogWarning($"API returned error: {response.StatusCode}");
                    return null;
                }

                try
                {
                    var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    
                    using (var doc = System.Text.Json.JsonDocument.Parse(responseContent))
                    {
                        var root = doc.RootElement;
                        
                        if (root.TryGetProperty("data", out var dataElement))
                        {
                            if (dataElement.ValueKind == System.Text.Json.JsonValueKind.Array)
                            {
                                var itemsJson = dataElement.GetRawText();
                                var users = System.Text.Json.JsonSerializer.Deserialize<List<UserViewModel>>(itemsJson, options) ?? new List<UserViewModel>();
                                _logger.LogInformation($"Search results: {users.Count}");
                                return users;
                            }
                        }
                    }
                }
                catch (Exception parseEx)
                {
                    _logger.LogWarning($"Error parsing API response: {parseEx.Message}");
                    return null;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error searching users: {ex.Message}");
                return null;
            }
        }

        public async Task<UserViewModel?> GetCurrentUserAsync()
        {
            try
            {
                var token = _tokenManager.GetToken();
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("No token available for GetCurrentUserAsync");
                    return null;
                }

                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                
                var apiUrl = GetApiBaseUrl();
                var endpoint = $"{apiUrl}/users/me";
                
                var response = await httpClient.GetAsync(endpoint);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation($"GET users/me - Status: {response.StatusCode}");
                
                if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(responseContent))
                {
                    _logger.LogWarning($"API returned error: {response.StatusCode}");
                    return null;
                }

                try
                {
                    var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    
                    using (var doc = System.Text.Json.JsonDocument.Parse(responseContent))
                    {
                        var root = doc.RootElement;
                        
                        if (root.TryGetProperty("data", out var dataElement) && 
                            dataElement.ValueKind == System.Text.Json.JsonValueKind.Object)
                        {
                            var itemsJson = dataElement.GetRawText();
                            var user = System.Text.Json.JsonSerializer.Deserialize<UserViewModel>(itemsJson, options);
                            _logger.LogInformation($"Current user loaded: {user?.Id}");
                            return user;
                        }
                    }
                }
                catch (Exception parseEx)
                {
                    _logger.LogWarning($"Error parsing API response: {parseEx.Message}");
                    return null;
                }

                _logger.LogWarning("Unable to extract current user from API response");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting current user: {ex.Message}");
                return null;
            }
        }

        public async Task<UserViewModel?> UpdateCurrentUserAsync(UpdateCurrentUserViewModel request)
        {
            try
            {
                var token = _tokenManager.GetToken();
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("No token available for UpdateCurrentUserAsync");
                    return null;
                }

                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                
                var apiUrl = GetApiBaseUrl();
                var endpoint = $"{apiUrl}/users/me";
                
                var jsonContent = new System.Net.Http.StringContent(
                    System.Text.Json.JsonSerializer.Serialize(request),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );
                
                var response = await httpClient.PutAsync(endpoint, jsonContent);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation($"PUT users/me - Status: {response.StatusCode}");
                
                if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(responseContent))
                {
                    _logger.LogWarning($"API returned error: {response.StatusCode}");
                    return null;
                }

                try
                {
                    var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    
                    using (var doc = System.Text.Json.JsonDocument.Parse(responseContent))
                    {
                        var root = doc.RootElement;
                        
                        if (root.TryGetProperty("data", out var dataElement) && 
                            dataElement.ValueKind == System.Text.Json.JsonValueKind.Object)
                        {
                            var itemsJson = dataElement.GetRawText();
                            var user = System.Text.Json.JsonSerializer.Deserialize<UserViewModel>(itemsJson, options);
                            _logger.LogInformation($"Current user updated: {user?.Id}");
                            return user;
                        }
                    }
                }
                catch (Exception parseEx)
                {
                    _logger.LogWarning($"Error parsing API response: {parseEx.Message}");
                    return null;
                }

                _logger.LogWarning("Unable to extract updated user from API response");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating current user: {ex.Message}");
                return null;
            }
        }

        public async Task<UserWithRolesViewModel?> GetUserDetailForAdminAsync(int id)
        {
            try
            {
                var token = _tokenManager.GetToken();
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("No token available for GetUserDetailForAdminAsync");
                    return null;
                }

                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                
                var apiUrl = GetApiBaseUrl();
                var endpoint = $"{apiUrl}/users/admin/{id}";
                
                var response = await httpClient.GetAsync(endpoint);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation($"GET users/admin/{id} - Status: {response.StatusCode}");
                
                if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(responseContent))
                {
                    _logger.LogWarning($"API returned error: {response.StatusCode}");
                    return null;
                }

                try
                {
                    var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    
                    using (var doc = System.Text.Json.JsonDocument.Parse(responseContent))
                    {
                        var root = doc.RootElement;
                        
                        if (root.TryGetProperty("data", out var dataElement) && 
                            dataElement.ValueKind == System.Text.Json.JsonValueKind.Object)
                        {
                            var itemsJson = dataElement.GetRawText();
                            var user = System.Text.Json.JsonSerializer.Deserialize<UserWithRolesViewModel>(itemsJson, options);
                            _logger.LogInformation($"User admin details loaded: {user?.Id}");
                            return user;
                        }
                    }
                }
                catch (Exception parseEx)
                {
                    _logger.LogWarning($"Error parsing API response: {parseEx.Message}");
                    return null;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting user detail for admin {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateUserRoleAsync(int userId, string roleName)
        {
            try
            {
                var token = _tokenManager.GetToken();
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("No token available for UpdateUserRoleAsync");
                    return false;
                }

                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                
                var apiUrl = GetApiBaseUrl();
                var endpoint = $"{apiUrl}/users/{userId}/roles";
                
                var requestBody = new { roleName = roleName };
                var jsonContent = new System.Net.Http.StringContent(
                    System.Text.Json.JsonSerializer.Serialize(requestBody),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );
                
                var response = await httpClient.PostAsync(endpoint, jsonContent);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation($"POST users/{userId}/roles - Status: {response.StatusCode}");
                
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating user role {userId}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> LockUserAsync(int userId, bool isLocked)
        {
            try
            {
                var token = _tokenManager.GetToken();
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("No token available for LockUserAsync");
                    return false;
                }

                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                
                var apiUrl = GetApiBaseUrl();
                var endpoint = $"{apiUrl}/users/{userId}/lock";
                
                var requestBody = new { isLocked = isLocked };
                var jsonContent = new System.Net.Http.StringContent(
                    System.Text.Json.JsonSerializer.Serialize(requestBody),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );
                
                var response = await httpClient.PostAsync(endpoint, jsonContent);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation($"POST users/{userId}/lock - Status: {response.StatusCode}");
                
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error locking user {userId}: {ex.Message}");
                return false;
            }
        }

        public async Task<UserStatisticsViewModel?> GetUserStatisticsAsync()
        {
            try
            {
                var token = _tokenManager.GetToken();
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("No token available for GetUserStatisticsAsync");
                    return null;
                }

                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                
                var apiUrl = GetApiBaseUrl();
                var endpoint = $"{apiUrl}/users/statistics/dashboard";
                
                var response = await httpClient.GetAsync(endpoint);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation($"GET users/statistics/dashboard - Status: {response.StatusCode}");
                
                if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(responseContent))
                {
                    _logger.LogWarning($"API returned error: {response.StatusCode}");
                    return null;
                }

                try
                {
                    var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    
                    using (var doc = System.Text.Json.JsonDocument.Parse(responseContent))
                    {
                        var root = doc.RootElement;
                        
                        if (root.TryGetProperty("data", out var dataElement) && 
                            dataElement.ValueKind == System.Text.Json.JsonValueKind.Object)
                        {
                            var itemsJson = dataElement.GetRawText();
                            var stats = System.Text.Json.JsonSerializer.Deserialize<UserStatisticsViewModel>(itemsJson, options);
                            _logger.LogInformation($"User statistics loaded");
                            return stats;
                        }
                    }
                }
                catch (Exception parseEx)
                {
                    _logger.LogWarning($"Error parsing API response: {parseEx.Message}");
                    return null;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting user statistics: {ex.Message}");
                return null;
            }
        }
    }
}
