using Microsoft.EntityFrameworkCore;
using mtkpm.Application.Common.Interfaces.Repositories;
using mtkpm.Domain.Entities.Business;
using mtkpm.Infrastructure.Data.Contexts;

namespace mtkpm.Infrastructure.Data.Repositories
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CartItem>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(ci => ci.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<CartItem>> GetByUserIdWithProductsAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(ci => ci.Product)
                    .ThenInclude(p => p.Category)
                .Where(ci => ci.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<CartItem?> GetByUserAndProductAsync(int userId, int productId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId, cancellationToken);
        }

        public async Task<CartItem?> GetByIdWithProductAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.Id == id, cancellationToken);
        }

        public async Task<int> GetCartItemCountAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(ci => ci.UserId == userId)
                .SumAsync(ci => ci.Quantity, cancellationToken);
        }

        public async Task RemoveByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            var cartItems = await _dbSet
                .Where(ci => ci.UserId == userId)
                .ToListAsync(cancellationToken);

            _dbSet.RemoveRange(cartItems);
        }
    }
}
