using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace mtkpm.Admin.Services
{
    public interface IBackendApiClient
    {
        Task<T> GetAsync<T>(string endpoint);
        Task<T> PostAsync<T>(string endpoint, object data);
        Task<T> PutAsync<T>(string endpoint, object data);
        Task<bool> DeleteAsync(string endpoint);
    }

    public class BackendApiClient : IBackendApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BackendApiClient> _logger;
        private readonly ITokenManager _tokenManager;
        private readonly string _apiBaseUrl;

        public BackendApiClient(HttpClient httpClient, IConfiguration configuration, ILogger<BackendApiClient> logger, ITokenManager tokenManager)
        {
            _httpClient = httpClient;
            _logger = logger;
            _tokenManager = tokenManager;
            _apiBaseUrl = configuration["Backend:ApiUrl"] ?? "https://localhost:5107/api";
        }

        private void AddAuthorizationHeader()
        {
            var token = _tokenManager.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                _logger.LogInformation($"Authorization header added - Token length: {token.Length}");
            }
            else
            {
                _logger.LogWarning("No token available - Authorization header NOT added");
            }
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            try
            {
                AddAuthorizationHeader();
                _logger.LogInformation($"Calling GET {_apiBaseUrl}/{endpoint}");
                
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{endpoint}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"GET {endpoint} - Status: {response.StatusCode}, Content Length: {content.Length}");
                    _logger.LogInformation($"Response Content: {content}");
                    
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    return JsonSerializer.Deserialize<T>(content, options);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"GET {endpoint} - Status: {response.StatusCode}, Error: {errorContent}");
                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calling GET {endpoint}");
                throw;
            }
        }

        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            try
            {
                AddAuthorizationHeader();
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync($"{_apiBaseUrl}/{endpoint}", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    return JsonSerializer.Deserialize<T>(responseContent, options);
                }

                _logger.LogError($"API error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calling POST {endpoint}");
                throw;
            }
        }

        public async Task<T> PutAsync<T>(string endpoint, object data)
        {
            try
            {
                AddAuthorizationHeader();
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PutAsync($"{_apiBaseUrl}/{endpoint}", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    return JsonSerializer.Deserialize<T>(responseContent, options);
                }

                _logger.LogError($"API error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calling PUT {endpoint}");
                throw;
            }
        }

        public async Task<(bool success, string errorMessage)> DeleteWithErrorAsync(string endpoint)
        {
            try
            {
                AddAuthorizationHeader();
                _logger.LogInformation($"Calling DELETE {_apiBaseUrl}/{endpoint}");
                
                var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/{endpoint}");
                var content = await response.Content.ReadAsStringAsync();
                
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"DELETE {endpoint} - Status: {response.StatusCode}");
                    return (true, "");
                }

                // Try to extract error message from API response
                var errorMessage = $"Lỗi {response.StatusCode}";
                
                if (!string.IsNullOrEmpty(content))
                {
                    try
                    {
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        using (JsonDocument doc = JsonDocument.Parse(content))
                        {
                            JsonElement root = doc.RootElement;
                            if (root.TryGetProperty("message", out JsonElement messageElement))
                            {
                                errorMessage = messageElement.GetString() ?? errorMessage;
                            }
                        }
                    }
                    catch { }
                }

                _logger.LogError($"DELETE {endpoint} - Status: {response.StatusCode}, Content: {content}");
                return (false, errorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calling DELETE {endpoint}");
                return (false, ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/{endpoint}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calling DELETE {endpoint}");
                throw;
            }
        }
    }
}
