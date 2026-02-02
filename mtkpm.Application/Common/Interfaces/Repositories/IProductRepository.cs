using mtkpm.Domain.Entities.Business;

namespace mtkpm.Application.Common.Interfaces.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product?> GetByIdWithCategoryAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetAllWithCategoryAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetAvailableProductsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    }
}
