using System.Text;
using System.Text.Json;
using mtkpm.Admin.Models;

namespace mtkpm.Admin.Infrastructure.Http
{
    /// <summary>
    /// Implementation of HTTP client wrapper with retry logic and logging
    /// </summary>
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpClientWrapper> _logger;
        private readonly HttpClientConfiguration _configuration;

        public HttpClientWrapper(HttpClient httpClient, ILogger<HttpClientWrapper> logger, HttpClientConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<T?> GetAsync<T>(string endpoint) where T : class
        {
            return await ExecuteWithRetryAsync(async () =>
            {
                _logger.LogInformation($"[GET] {endpoint}");
                var response = await _httpClient.GetAsync(endpoint);
                return await HandleResponseAsync<T>(response);
            });
        }

        public async Task<T?> PostAsync<T>(string endpoint, object? data = null) where T : class
        {
            return await ExecuteWithRetryAsync(async () =>
            {
                _logger.LogInformation($"[POST] {endpoint}");
                var content = data != null ? new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json") : null;
                var response = await _httpClient.PostAsync(endpoint, content);
                return await HandleResponseAsync<T>(response);
            });
        }

        public async Task<T?> PutAsync<T>(string endpoint, object? data = null) where T : class
        {
            return await ExecuteWithRetryAsync(async () =>
            {
                _logger.LogInformation($"[PUT] {endpoint}");
                var content = data != null ? new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json") : null;
                var response = await _httpClient.PutAsync(endpoint, content);
                return await HandleResponseAsync<T>(response);
            });
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            try
            {
                _logger.LogInformation($"[DELETE] {endpoint}");
                var response = await _httpClient.DeleteAsync(endpoint);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Delete failed: {ex.Message}");
                return false;
            }
        }

        public void SetAuthorizationHeader(string? token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                _logger.LogInformation("Authorization header set");
            }
        }

        public void ClearAuthorizationHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            _logger.LogInformation("Authorization header cleared");
        }

        private async Task<T?> ExecuteWithRetryAsync<T>(Func<Task<T?>> operation) where T : class
        {
            for (int attempt = 1; attempt <= _configuration.MaxRetries; attempt++)
            {
                try
                {
                    return await operation();
                }
                catch (HttpRequestException ex) when (attempt < _configuration.MaxRetries)
                {
                    _logger.LogWarning($"Request failed (attempt {attempt}/{_configuration.MaxRetries}): {ex.Message}");
                    await Task.Delay(1000 * attempt); // Exponential backoff
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Request error: {ex.Message}");
                    return null;
                }
            }
            return null;
        }

        private async Task<T?> HandleResponseAsync<T>(HttpResponseMessage response) where T : class
        {
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Response failed with status {response.StatusCode}: {content}");
                return null;
            }

            try
            {
                // Try to deserialize as ApiResponse wrapper
                var wrappedResult = JsonSerializer.Deserialize<ApiResponse<T>>(content);
                if (wrappedResult?.Data != null)
                {
                    _logger.LogInformation($"Successfully deserialized {typeof(T).Name} from wrapper");
                    return wrappedResult.Data;
                }

                // Try to deserialize directly as T
                var directResult = JsonSerializer.Deserialize<T>(content);
                if (directResult != null)
                {
                    _logger.LogInformation($"Successfully deserialized {typeof(T).Name} directly");
                    return directResult;
                }

                _logger.LogWarning($"Could not deserialize response to {typeof(T).Name}");
                return null;
            }
            catch (JsonException ex)
            {
                _logger.LogError($"JSON deserialization error: {ex.Message}");
                return null;
            }
        }
    }
}
