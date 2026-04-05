using mtkpm.Admin.Features.Orders.Models;

namespace mtkpm.Admin.Features.Orders.Services
{
    /// <summary>
    /// Interface for order management service
    /// </summary>
    public interface IOrderFeatureService
    {
        /// <summary>
        /// Get paginated orders list
        /// </summary>
        Task<(List<OrderDto> Items, int TotalCount)?> GetOrdersAsync(int pageIndex, int pageSize, string? status = null, DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Get order detail by ID
        /// </summary>
        Task<OrderDetailDto?> GetOrderByIdAsync(int id);

        /// <summary>
        /// Get order items
        /// </summary>
        Task<List<OrderItemDto>?> GetOrderItemsAsync(int orderId);

        /// <summary>
        /// Update order status
        /// </summary>
        Task<bool> UpdateOrderStatusAsync(int id, string status);

        /// <summary>
        /// Cancel order
        /// </summary>
        Task<bool> CancelOrderAsync(int id);

        /// <summary>
        /// Get today's orders count
        /// </summary>
        Task<int> GetTodayOrdersCountAsync();

        /// <summary>
        /// Get total revenue
        /// </summary>
        Task<decimal> GetTotalRevenueAsync();
    }
}
