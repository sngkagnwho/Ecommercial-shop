using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using mtkpm.Admin.Configuration;

namespace mtkpm.Admin.Services
{
    /// <summary>
    /// Interface for JWT token management
    /// </summary>
    public interface ITokenManager
    {
        /// <summary>
        /// Save token to session/storage
        /// </summary>
        void SaveToken(string token, string refreshToken);

        /// <summary>
        /// Get stored token
        /// </summary>
        string? GetToken();

        /// <summary>
        /// Get refresh token
        /// </summary>
        string? GetRefreshToken();

        /// <summary>
        /// Check if token is valid and not expired
        /// </summary>
        bool IsTokenValid();

        /// <summary>
        /// Get user ID from token claims
        /// </summary>
        int? GetUserId();

        /// <summary>
        /// Get user email from token claims
        /// </summary>
        string? GetUserEmail();

        /// <summary>
        /// Get user roles from token claims
        /// </summary>
        List<string> GetUserRoles();

        /// <summary>
        /// Clear stored tokens
        /// </summary>
        void ClearTokens();
    }

    /// <summary>
    /// Implementation of token manager using session storage
    /// </summary>
    public class TokenManager : ITokenManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<TokenManager> _logger;
        private const string TokenKey = "AdminToken";
        private const string RefreshTokenKey = "AdminRefreshToken";

        public TokenManager(IHttpContextAccessor httpContextAccessor, ILogger<TokenManager> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public void SaveToken(string token, string refreshToken)
        {
            try
            {
                var session = _httpContextAccessor.HttpContext?.Session;
                if (session != null)
                {
                    session.SetString(TokenKey, token);
                    session.SetString(RefreshTokenKey, refreshToken);
                    _logger.LogInformation("Token saved to session");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving token: {ex.Message}");
            }
        }

        public string? GetToken()
        {
            return _httpContextAccessor.HttpContext?.Session.GetString(TokenKey);
        }

        public string? GetRefreshToken()
        {
            return _httpContextAccessor.HttpContext?.Session.GetString(RefreshTokenKey);
        }

        public bool IsTokenValid()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token))
                return false;

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                return jwtToken.ValidTo > DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error validating token: {ex.Message}");
                return false;
            }
        }

        public int? GetUserId()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token))
                return null;

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == ClaimTypes.NameIdentifier);
                
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
                    return userId;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error extracting user ID: {ex.Message}");
            }

            return null;
        }

        public string? GetUserEmail()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token))
                return null;

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                return jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error extracting email: {ex.Message}");
            }

            return null;
        }

        public List<string> GetUserRoles()
        {
            var token = GetToken();
            var roles = new List<string>();

            if (string.IsNullOrEmpty(token))
                return roles;

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var roleClaims = jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role);
                roles.AddRange(roleClaims.Select(c => c.Value));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error extracting roles: {ex.Message}");
            }

            return roles;
        }

        public void ClearTokens()
        {
            try
            {
                var session = _httpContextAccessor.HttpContext?.Session;
                session?.Remove(TokenKey);
                session?.Remove(RefreshTokenKey);
                _logger.LogInformation("Tokens cleared from session");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error clearing tokens: {ex.Message}");
            }
        }
    }
}
