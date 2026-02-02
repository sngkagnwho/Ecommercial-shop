using Microsoft.EntityFrameworkCore;
using mtkpm.Application.Common.Interfaces.Repositories;
using mtkpm.Domain.Entities.Business;
using mtkpm.Infrastructure.Data.Contexts;

namespace mtkpm.Infrastructure.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Product?> GetByIdWithCategoryAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetAllWithCategoryAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(p => p.Category)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetAvailableProductsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.StockQuantity > 0 && !p.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            var term = searchTerm.ToLower();
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.Name.ToLower().Contains(term) 
                         || p.Description.ToLower().Contains(term))
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(p => p.Id == id, cancellationToken);
        }
    }
}
