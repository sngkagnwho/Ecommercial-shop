using mtkpm.Domain.Entities.Business;

namespace mtkpm.Application.Common.Interfaces.Repositories
{
    public interface ICartItemRepository : IRepository<CartItem>
    {
        Task<IEnumerable<CartItem>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<CartItem>> GetByUserIdWithProductsAsync(int userId, CancellationToken cancellationToken = default);
        Task<CartItem?> GetByUserAndProductAsync(int userId, int productId, CancellationToken cancellationToken = default);
        Task<CartItem?> GetByIdWithProductAsync(int id, CancellationToken cancellationToken = default);
        Task<int> GetCartItemCountAsync(int userId, CancellationToken cancellationToken = default);
        Task RemoveByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    }
}
