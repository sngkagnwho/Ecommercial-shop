using Microsoft.EntityFrameworkCore;
using mtkpm.Application.Common.Interfaces.Repositories;
using mtkpm.Domain.Entities.Identity_Auth;
using mtkpm.Infrastructure.Data.Contexts;

namespace mtkpm.Infrastructure.Data.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
        }

        public async Task<IEnumerable<RefreshToken>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(rt => rt.UserId == userId)
                .OrderByDescending(rt => rt.CreateAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            return await _dbSet
                .Where(rt => rt.UserId == userId 
                          && rt.ExpiresAt > now 
                          && rt.RevokedAt == null)
                .ToListAsync(cancellationToken);
        }

        public async Task RevokeAllUserTokensAsync(int userId, CancellationToken cancellationToken = default)
        {
            var tokens = await GetActiveTokensByUserIdAsync(userId, cancellationToken);
            
            foreach (var token in tokens)
            {
                token.Revoke(null, "All tokens revoked");
            }
        }

        public async Task CleanupExpiredTokensAsync(CancellationToken cancellationToken = default)
        {
            var expiredTokens = await _dbSet
                .Where(rt => rt.ExpiresAt < DateTime.UtcNow)
                .ToListAsync(cancellationToken);

            _dbSet.RemoveRange(expiredTokens);
        }

        public async Task<bool> IsTokenValidAsync(string token, CancellationToken cancellationToken = default)
        {
            var refreshToken = await GetByTokenAsync(token, cancellationToken);
            return refreshToken != null && refreshToken.IsActive;
        }
    }
}
