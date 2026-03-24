using mtkpm.Domain.Entities.Business;

namespace mtkpm.Application.Common.Interfaces.Repositories
{
    public interface IFavouriteProductRepository : IRepository<FavouriteProduct>
    {
        Task<IEnumerable<FavouriteProduct>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<FavouriteProduct>> GetByUserIdWithProductsAsync(int userId, CancellationToken cancellationToken = default);
        Task<FavouriteProduct?> GetByUserAndProductAsync(int userId, int productId, CancellationToken cancellationToken = default);
        Task<FavouriteProduct?> GetByIdWithProductAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> IsFavouriteAsync(int userId, int productId, CancellationToken cancellationToken = default);
        Task RemoveByUserAndProductAsync(int userId, int productId, CancellationToken cancellationToken = default);
    }
}
