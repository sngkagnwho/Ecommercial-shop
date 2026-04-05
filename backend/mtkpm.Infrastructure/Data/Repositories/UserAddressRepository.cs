using Microsoft.EntityFrameworkCore;
using mtkpm.Application.Common.Interfaces.Repositories;
using mtkpm.Domain.Entities.Business;
using mtkpm.Infrastructure.Data.Contexts;

namespace mtkpm.Infrastructure.Data.Repositories
{
    public class UserAddressRepository : Repository<UserAddress>, IUserAddressRepository
    {
        public UserAddressRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserAddress>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(ua => ua.UserId == userId)
                .OrderByDescending(ua => ua.IsDefault)
                .ThenByDescending(ua => ua.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<UserAddress?> GetDefaultAddressByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(ua => ua.UserId == userId && ua.IsDefault, cancellationToken);
        }

        public async Task<UserAddress?> GetByIdAndUserIdAsync(int addressId, int userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(ua => ua.Id == addressId && ua.UserId == userId, cancellationToken);
        }
    }
}
