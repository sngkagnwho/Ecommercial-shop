using mtkpm.Admin.Constants;
using mtkpm.Admin.Models.Auth;

namespace mtkpm.Admin.Services
{
    /// <summary>
    /// Interface for authentication service
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Login admin user
        /// </summary>
        Task<LoginResponse?> LoginAsync(string username, string password);

        /// <summary>
        /// Logout current user
        /// </summary>
        Task LogoutAsync();

        /// <summary>
        /// Refresh access token
        /// </summary>
        Task<LoginResponse?> RefreshTokenAsync();

        /// <summary>
        /// Check if user is authenticated
        /// </summary>
        bool IsAuthenticated();

        /// <summary>
        /// Check if user has admin role
        /// </summary>
        bool IsAdmin();

        /// <summary>
        /// Get current admin user info
        /// </summary>
        AdminUserViewModel? GetCurrentUser();
    }

    /// <summary>
    /// Implementation of authentication service
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IApiService _apiService;
        private readonly ITokenManager _tokenManager;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IApiService apiService, ITokenManager tokenManager, ILogger<AuthService> logger)
        {
            _apiService = apiService;
            _tokenManager = tokenManager;
            _logger = logger;
        }

        public async Task<LoginResponse?> LoginAsync(string username, string password)
        {
            try
            {
                _logger.LogInformation($"[LoginAsync] Starting login for user: {username}");
                
                var loginRequest = new LoginRequest 
                { 
                    UserNameOrEmail = username, 
                    Password = password,
                    RememberMe = true
                };
                _logger.LogInformation($"[LoginAsync] Created LoginRequest with UserNameOrEmail={username}");
                
                var apiResponse = await _apiService.PostAsync<LoginApiResponse>(ApiEndpoints.Auth.Login, loginRequest);
                
                _logger.LogInformation($"[LoginAsync] API Response received. apiResponse={apiResponse}, Success={apiResponse?.Success}, User={apiResponse?.User}");

                if (apiResponse?.Success == true && apiResponse.User != null)
                {
                    _logger.LogInformation($"[LoginAsync] Success! User ID={apiResponse.User.Id}, UserName={apiResponse.User.UserName}, Roles={string.Join(",", apiResponse.User.Roles)}");
                    
                    // Convert API response to LoginResponse
                    var response = new LoginResponse
                    {
                        UserId = apiResponse.User.Id,
                        UserName = apiResponse.User.UserName,
                        Email = apiResponse.User.Email,
                        FullName = apiResponse.User.UserName,
                        Roles = apiResponse.User.Roles,
                        AccessToken = apiResponse.AccessToken,
                        RefreshToken = apiResponse.RefreshToken,
                        ExpiresIn = (int)(apiResponse.ExpiresAt - DateTime.UtcNow).TotalSeconds
                    };

                    _tokenManager.SaveToken(response.AccessToken, response.RefreshToken);
                    _apiService.SetAuthorizationHeader(response.AccessToken);
                    _logger.LogInformation($"[LoginAsync] Tokens saved and auth header set. User {username} logged in successfully");
                    return response;
                }
                else
                {
                    _logger.LogWarning($"[LoginAsync] Login failed: apiResponse={apiResponse}, Success={apiResponse?.Success}, User={apiResponse?.User}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[LoginAsync] Login error: {ex.Message}, StackTrace: {ex.StackTrace}");
            }

            return null;
        }

        public async Task LogoutAsync()
        {
            try
            {
                await _apiService.PostAsync<object>(ApiEndpoints.Auth.Logout);
                _tokenManager.ClearTokens();
                _apiService.ClearAuthorizationHeader();
                _logger.LogInformation("User logged out");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Logout error: {ex.Message}");
            }
        }

        public async Task<LoginResponse?> RefreshTokenAsync()
        {
            try
            {
                var refreshToken = _tokenManager.GetRefreshToken();
                if (string.IsNullOrEmpty(refreshToken))
                    return null;

                var response = await _apiService.PostAsync<LoginResponse>(
                    ApiEndpoints.Auth.RefreshToken,
                    new { refreshToken }
                );

                if (response != null)
                {
                    _tokenManager.SaveToken(response.AccessToken, response.RefreshToken);
                    _apiService.SetAuthorizationHeader(response.AccessToken);
                    _logger.LogInformation("Token refreshed successfully");
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Token refresh error: {ex.Message}");
            }

            return null;
        }

        public bool IsAuthenticated()
        {
            return _tokenManager.IsTokenValid();
        }

        public bool IsAdmin()
        {
            var roles = _tokenManager.GetUserRoles();
            return roles.Contains("Admin", StringComparer.OrdinalIgnoreCase);
        }

        public AdminUserViewModel? GetCurrentUser()
        {
            var userId = _tokenManager.GetUserId();
            var email = _tokenManager.GetUserEmail();
            var roles = _tokenManager.GetUserRoles();

            if (!userId.HasValue || string.IsNullOrEmpty(email))
                return null;

            return new AdminUserViewModel
            {
                Id = userId.Value,
                Email = email,
                Roles = roles,
                IsActive = true
            };
        }
    }
}
