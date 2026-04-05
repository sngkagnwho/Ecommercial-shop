using mtkpm.Admin.Features.Dashboard.Models;

namespace mtkpm.Admin.Features.Dashboard.Services
{
    /// <summary>
    /// Interface for dashboard analytics service
    /// </summary>
    public interface IDashboardService
    {
        /// <summary>
        /// Get dashboard statistics
        /// </summary>
        Task<DashboardStats?> GetDashboardStatsAsync();

        /// <summary>
        /// Get revenue by date range
        /// </summary>
        Task<List<RevenueChartData>?> GetRevenueChartAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Get order trends
        /// </summary>
        Task<List<OrderTrend>?> GetOrderTrendsAsync(int days = 30);

        /// <summary>
        /// Get top products
        /// </summary>
        Task<List<TopProduct>?> GetTopProductsAsync(int count = 10);
    }
}
