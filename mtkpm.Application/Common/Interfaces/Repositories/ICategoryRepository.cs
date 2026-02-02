using mtkpm.Domain.Entities.Business;

namespace mtkpm.Application.Common.Interfaces.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category?> GetByIdWithProductsAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Category>> GetAllWithProductCountAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
        Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
