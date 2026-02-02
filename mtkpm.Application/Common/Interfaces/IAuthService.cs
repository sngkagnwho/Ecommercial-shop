using mtkpm.Application.Common.DTOs.Auth;

namespace mtkpm.Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterDto request, string? ipAddress = null, string? deviceInfo = null);
        Task<AuthResponse> LoginAsync(LoginDto request, string? ipAddress = null, string? deviceInfo = null);
        Task<AuthResponse> RefreshTokenAsync(RefreshTokenDto request, string? ipAddress = null, string? deviceInfo = null);
        Task<bool> RevokeTokenAsync(string refreshToken, string? ipAddress = null);
        Task<bool> RevokeAllTokensAsync(int userId);
        Task<bool> LogoutAsync(int userId, string refreshToken);
    }
}
