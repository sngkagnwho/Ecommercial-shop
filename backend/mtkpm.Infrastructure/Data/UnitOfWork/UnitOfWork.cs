using Microsoft.EntityFrameworkCore.Storage;
using mtkpm.Application.Common.Interfaces.Repositories;
using mtkpm.Infrastructure.Data.Contexts;

namespace mtkpm.Infrastructure.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(
            ApplicationDbContext context,
            IProductRepository products,
            ICategoryRepository categories,
            IOrderRepository orders,
            ICartItemRepository cartItems,
            IFavouriteProductRepository favouriteProducts,
            IRefreshTokenRepository refreshTokens,
            IUserAddressRepository userAddresses)
        {
            _context = context;
            Products = products;
            Categories = categories;
            Orders = orders;
            CartItems = cartItems;
            FavouriteProducts = favouriteProducts;
            RefreshTokens = refreshTokens;
            UserAddresses = userAddresses;
        }

        public IProductRepository Products { get; }
        public ICategoryRepository Categories { get; }
        public IOrderRepository Orders { get; }
        public ICartItemRepository CartItems { get; }
        public IFavouriteProductRepository FavouriteProducts { get; }
        public IRefreshTokenRepository RefreshTokens { get; }
        public IUserAddressRepository UserAddresses { get; }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                if (_transaction != null)
                {
                    await _transaction.CommitAsync(cancellationToken);
                }
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(cancellationToken);
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
