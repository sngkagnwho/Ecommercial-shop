using mtkpm.Domain.Entities.Identity_Auth;

namespace mtkpm.Application.Common.Interfaces
{
    public interface ITokenService
    {
        Task<RefreshToken> CreateRefreshTokenAsync(int userId, string token, string? ipAddress, string? deviceInfo);
        Task<RefreshToken?> GetRefreshTokenAsync(string token);
        Task<bool> ValidateRefreshTokenAsync(string token);
        Task RevokeRefreshTokenAsync(string token, string? ipAddress);
        Task RevokeAllUserTokensAsync(int userId);
        Task<bool> IsTokenFamilyValidAsync(string token);
        Task CleanupExpiredTokensAsync();
    }
}
