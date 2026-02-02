using Microsoft.EntityFrameworkCore;
using mtkpm.Application.Common.Interfaces.Repositories;
using mtkpm.Domain.Entities.Business;
using mtkpm.Infrastructure.Data.Contexts;

namespace mtkpm.Infrastructure.Data.Repositories
{
    public class FavouriteProductRepository : Repository<FavouriteProduct>, IFavouriteProductRepository
    {
        public FavouriteProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<FavouriteProduct>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(fp => fp.UserId == userId)
                .OrderByDescending(fp => fp.AddedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<FavouriteProduct>> GetByUserIdWithProductsAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(fp => fp.Product)
                    .ThenInclude(p => p.Category)
                .Where(fp => fp.UserId == userId)
                .OrderByDescending(fp => fp.AddedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<FavouriteProduct?> GetByUserAndProductAsync(int userId, int productId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(fp => fp.Product)
                .FirstOrDefaultAsync(fp => fp.UserId == userId && fp.ProductId == productId, cancellationToken);
        }

        public async Task<FavouriteProduct?> GetByIdWithProductAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(fp => fp.Product)
                .FirstOrDefaultAsync(fp => fp.Id == id, cancellationToken);
        }

        public async Task<bool> IsFavouriteAsync(int userId, int productId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AnyAsync(fp => fp.UserId == userId && fp.ProductId == productId, cancellationToken);
        }

        public async Task RemoveByUserAndProductAsync(int userId, int productId, CancellationToken cancellationToken = default)
        {
            var favourite = await _dbSet
                .FirstOrDefaultAsync(fp => fp.UserId == userId && fp.ProductId == productId, cancellationToken);

            if (favourite != null)
            {
                _dbSet.Remove(favourite);
            }
        }
    }
}
