using mtkpm.Application.Common.DTOs.Common;
using mtkpm.Application.Common.DTOs.Order;
using mtkpm.Domain.Enums.Business;

namespace mtkpm.Application.Common.Interfaces.Services
{
    public interface IOrderService
    {
        Task<OrderDto?> GetByIdAsync(int id);
        Task<OrderDto?> GetByOrderNumberAsync(string orderNumber);
        Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId);
        Task<PaginatedListDto<OrderDto>> GetPaginatedAsync(int pageIndex, int pageSize, int? userId = null, OrderStatus? status = null);
        Task<OrderDto> CreateAsync(int userId, CreateOrderDto dto);
        Task<bool> UpdateStatusAsync(int id, OrderStatus status);
        Task<bool> CancelOrderAsync(int id, int userId);
        Task<bool> MarkAsPaidAsync(int id);
    }
}
