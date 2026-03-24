using mtkpm.Admin.Constants;
using mtkpm.Admin.Models.Order;

namespace mtkpm.Admin.Services
{
    /// <summary>
    /// Interface for order management service
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Get all orders with pagination
        /// </summary>
        Task<Models.PaginatedResponse<OrderViewModel>?> GetOrdersAsync(int pageIndex, int pageSize);

        /// <summary>
        /// Get order by ID
        /// </summary>
        Task<OrderViewModel?> GetOrderByIdAsync(int id);

        /// <summary>
        /// Get order by order number
        /// </summary>
        Task<OrderViewModel?> GetOrderByNumberAsync(string orderNumber);

        /// <summary>
        /// Update order status
        /// </summary>
        Task<bool> UpdateOrderStatusAsync(int id, string status, string? note = null);

        /// <summary>
        /// Mark order as paid
        /// </summary>
        Task<bool> MarkOrderAsPaidAsync(int id);

        /// <summary>
        /// Cancel order
        /// </summary>
        Task<bool> CancelOrderAsync(int id);

        /// <summary>
        /// Get orders by user
        /// </summary>
        Task<List<OrderViewModel>?> GetUserOrdersAsync(int userId);
    }

    /// <summary>
    /// Implementation of order service
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IApiService apiService, ILogger<OrderService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<Models.PaginatedResponse<OrderViewModel>?> GetOrdersAsync(int pageIndex, int pageSize)
        {
            try
            {
                var endpoint = $"/orders?pageIndex={pageIndex}&pageSize={pageSize}";
                return await _apiService.GetAsync<Models.PaginatedResponse<OrderViewModel>>(endpoint);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting orders: {ex.Message}");
                return null;
            }
        }

        public async Task<OrderViewModel?> GetOrderByIdAsync(int id)
        {
            try
            {
                return await _apiService.GetAsync<OrderViewModel>($"/orders/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting order {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<OrderViewModel?> GetOrderByNumberAsync(string orderNumber)
        {
            try
            {
                return await _apiService.GetAsync<OrderViewModel>($"/orders/number/{orderNumber}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting order by number {orderNumber}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateOrderStatusAsync(int id, string status, string? note = null)
        {
            try
            {
                var request = new { status, note };
                var result = await _apiService.PutAsync<object>($"/orders/{id}/status", request);
                return result != null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating order {id} status: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> MarkOrderAsPaidAsync(int id)
        {
            try
            {
                var result = await _apiService.PutAsync<object>($"/orders/{id}/mark-as-paid", null);
                return result != null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error marking order {id} as paid: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CancelOrderAsync(int id)
        {
            try
            {
                return await _apiService.DeleteAsync($"/orders/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error canceling order {id}: {ex.Message}");
                return false;
            }
        }

        public async Task<List<OrderViewModel>?> GetUserOrdersAsync(int userId)
        {
            try
            {
                return await _apiService.GetAsync<List<OrderViewModel>>($"/orders/user/{userId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting user {userId} orders: {ex.Message}");
                return null;
            }
        }
    }
}
