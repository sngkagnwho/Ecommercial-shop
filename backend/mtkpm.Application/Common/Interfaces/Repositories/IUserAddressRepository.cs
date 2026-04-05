using mtkpm.Domain.Entities.Business;

namespace mtkpm.Application.Common.Interfaces.Repositories
{
    public interface IUserAddressRepository : IRepository<UserAddress>
    {
        Task<IEnumerable<UserAddress>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
        Task<UserAddress?> GetDefaultAddressByUserIdAsync(int userId, CancellationToken cancellationToken = default);
        Task<UserAddress?> GetByIdAndUserIdAsync(int addressId, int userId, CancellationToken cancellationToken = default);
    }
}
