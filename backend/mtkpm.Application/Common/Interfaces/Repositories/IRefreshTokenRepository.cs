using mtkpm.Domain.Entities.Identity_Auth;

namespace mtkpm.Application.Common.Interfaces.Repositories
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
        Task<IEnumerable<RefreshToken>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(int userId, CancellationToken cancellationToken = default);
        Task RevokeAllUserTokensAsync(int userId, CancellationToken cancellationToken = default);
        Task CleanupExpiredTokensAsync(CancellationToken cancellationToken = default);
        Task<bool> IsTokenValidAsync(string token, CancellationToken cancellationToken = default);
    }
}
