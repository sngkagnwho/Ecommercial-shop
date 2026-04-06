using mtkpm.Admin.Models;

namespace mtkpm.Admin.Services
{
    /// <summary>
    /// Interface for making HTTP requests to the backend API
    /// </summary>
    public interface IApiService
    {
        /// <summary>
        /// Send GET request
        /// </summary>
        Task<T?> GetAsync<T>(string endpoint) where T : class;

        /// <summary>
        /// Send POST request
        /// </summary>
        Task<T?> PostAsync<T>(string endpoint, object? data = null) where T : class;

        /// <summary>
        /// Send PUT request
        /// </summary>
        Task<T?> PutAsync<T>(string endpoint, object? data = null) where T : class;

        /// <summary>
        /// Send DELETE request
        /// </summary>
        Task<bool> DeleteAsync(string endpoint);

        /// <summary>
        /// Set authorization header with JWT token
        /// </summary>
        void SetAuthorizationHeader(string token);

        /// <summary>
        /// Clear authorization header
        /// </summary>
        void ClearAuthorizationHeader();
    }

    /// <summary>
    /// Implementation of API service using HttpClient
    /// </summary>
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiService> _logger;
        private static readonly System.Text.Json.JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<T?> GetAsync<T>(string endpoint) where T : class
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"GET request failed: {response.StatusCode} - {endpoint}");
                    return default;
                }

                var content = await response.Content.ReadAsStringAsync();
                try
                {
                    // First try to deserialize as ApiResponse<T> wrapper (backend always wraps responses)
                    var wrappedResult = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<T>>(content, JsonOptions);
                    if (wrappedResult?.Data != null)
                    {
                        _logger.LogInformation($"Successfully deserialized {typeof(T).Name} from ApiResponse wrapper");
                        return wrappedResult.Data;
                    }

                    // If that fails, try to deserialize directly as T (for cases without wrapper)
                    var directResult = System.Text.Json.JsonSerializer.Deserialize<T>(content, JsonOptions);
                    if (directResult != null)
                    {
                        _logger.LogInformation($"Successfully deserialized {typeof(T).Name} directly");
                        return directResult;
                    }

                    _logger.LogWarning($"Response could not be deserialized to {typeof(T).Name}");
                    return default;
                }
                catch (System.Text.Json.JsonException jsonEx)
                {
                    _logger.LogError($"JSON deserialization error for {typeof(T).Name}: {jsonEx.Message}. Response: {content}");
                    return default;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GET request: {ex.Message}");
                return default;
            }
        }

        public async Task<T?> PostAsync<T>(string endpoint, object? data = null) where T : class
        {
            try
            {
                var content = data != null 
                    ? new StringContent(System.Text.Json.JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json")
                    : null;

                var response = await _httpClient.PostAsync(endpoint, content);

                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"POST {endpoint} - Status: {response.StatusCode}");
                _logger.LogInformation($"Response Body: {responseContent}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"POST request failed: {response.StatusCode} - {endpoint}");
                    return default;
                }

                try
                {
                    // First try to deserialize as ApiResponse<T> wrapper
                    var wrappedResult = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<T>>(responseContent, JsonOptions);
                    if (wrappedResult?.Data != null)
                    {
                        _logger.LogInformation($"Successfully deserialized {typeof(T).Name} from ApiResponse wrapper");
                        return wrappedResult.Data;
                    }

                    // If that fails, try to deserialize directly as T
                    var directResult = System.Text.Json.JsonSerializer.Deserialize<T>(responseContent, JsonOptions);
                    if (directResult != null)
                    {
                        _logger.LogInformation($"Successfully deserialized {typeof(T).Name} directly");
                        return directResult;
                    }

                    _logger.LogWarning($"Response could not be deserialized to {typeof(T).Name}");
                    return default;
                }
                catch (System.Text.Json.JsonException jsonEx)
                {
                    _logger.LogError($"JSON deserialization error for {typeof(T).Name}: {jsonEx.Message}. Response: {responseContent}");
                    return default;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in POST request: {ex.Message}");
                return default;
            }
        }

        public async Task<T?> PutAsync<T>(string endpoint, object? data = null) where T : class
        {
            try
            {
                var content = data != null
                    ? new StringContent(System.Text.Json.JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json")
                    : null;

                var response = await _httpClient.PutAsync(endpoint, content);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"PUT request failed: {response.StatusCode} - {endpoint}");
                    return default;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                try
                {
                    // First try to deserialize as ApiResponse<T> wrapper
                    var wrappedResult = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<T>>(responseContent, JsonOptions);
                    if (wrappedResult?.Data != null)
                    {
                        _logger.LogInformation($"Successfully deserialized {typeof(T).Name} from ApiResponse wrapper");
                        return wrappedResult.Data;
                    }

                    // If that fails, try to deserialize directly as T
                    var directResult = System.Text.Json.JsonSerializer.Deserialize<T>(responseContent, JsonOptions);
                    if (directResult != null)
                    {
                        _logger.LogInformation($"Successfully deserialized {typeof(T).Name} directly");
                        return directResult;
                    }

                    _logger.LogWarning($"Response could not be deserialized to {typeof(T).Name}");
                    return default;
                }
                catch (System.Text.Json.JsonException jsonEx)
                {
                    _logger.LogError($"JSON deserialization error for {typeof(T).Name}: {jsonEx.Message}. Response: {responseContent}");
                    return default;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in PUT request: {ex.Message}");
                return default;
            }
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(endpoint);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"DELETE request failed: {response.StatusCode} - {endpoint}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DELETE request: {ex.Message}");
                return false;
            }
        }

        public void SetAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        public void ClearAuthorizationHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
