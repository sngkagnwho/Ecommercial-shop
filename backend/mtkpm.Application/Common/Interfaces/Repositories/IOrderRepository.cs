using mtkpm.Domain.Entities.Business;
using mtkpm.Domain.Enums.Business;

namespace mtkpm.Application.Common.Interfaces.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
        Task<IEnumerable<Order>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default);
        Task<Order?> GetWithDetailsAsync(int orderId, CancellationToken cancellationToken = default);
    }
}
