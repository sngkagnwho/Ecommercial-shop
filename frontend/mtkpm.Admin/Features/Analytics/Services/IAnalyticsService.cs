using mtkpm.Admin.Features.Analytics.Models;

namespace mtkpm.Admin.Features.Analytics.Services
{
    /// <summary>
    /// Interface for analytics service
    /// </summary>
    public interface IAnalyticsService
    {
        /// <summary>
        /// Get analytics report for date range
        /// </summary>
        Task<AnalyticsReport?> GetAnalyticsReportAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Get category performance
        /// </summary>
        Task<List<CategoryPerformance>?> GetCategoryPerformanceAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Get hourly breakdown
        /// </summary>
        Task<List<HourlyData>?> GetHourlyBreakdownAsync(DateTime date);

        /// <summary>
        /// Export analytics as CSV
        /// </summary>
        Task<byte[]?> ExportAnalyticsAsync(DateTime startDate, DateTime endDate);
    }
}
