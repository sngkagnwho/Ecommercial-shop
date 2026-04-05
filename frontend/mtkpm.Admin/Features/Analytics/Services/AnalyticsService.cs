using mtkpm.Admin.Features.Analytics.Models;
using mtkpm.Admin.Infrastructure.Http;
using mtkpm.Admin.Infrastructure.Caching;

namespace mtkpm.Admin.Features.Analytics.Services
{
    /// <summary>
    /// Implementation of analytics service
    /// </summary>
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IHttpClientWrapper _httpClient;
        private readonly ICacheService _cache;
        private readonly ILogger<AnalyticsService> _logger;

        public AnalyticsService(IHttpClientWrapper httpClient, ICacheService cache, ILogger<AnalyticsService> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
        }

        public async Task<AnalyticsReport?> GetAnalyticsReportAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var cacheKey = $"analytics_report_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}";
                var cached = _cache.Get<AnalyticsReport>(cacheKey);
                if (cached != null)
                    return cached;

                var endpoint = $"analytics/report?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";
                var report = await _httpClient.GetAsync<AnalyticsReport>(endpoint);

                if (report != null)
                {
                    _cache.Set(cacheKey, report, TimeSpan.FromHours(1));
                    _logger.LogInformation($"Analytics report retrieved for {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");
                }

                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting analytics report: {ex.Message}");
                return null;
            }
        }

        public async Task<List<CategoryPerformance>?> GetCategoryPerformanceAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var endpoint = $"analytics/category-performance?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";
                var performance = await _httpClient.GetAsync<List<CategoryPerformance>>(endpoint);
                return performance;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting category performance: {ex.Message}");
                return null;
            }
        }

        public async Task<List<HourlyData>?> GetHourlyBreakdownAsync(DateTime date)
        {
            try
            {
                var cacheKey = $"analytics_hourly_{date:yyyyMMdd}";
                var cached = _cache.Get<List<HourlyData>>(cacheKey);
                if (cached != null)
                    return cached;

                var endpoint = $"analytics/hourly-breakdown?date={date:yyyy-MM-dd}";
                var data = await _httpClient.GetAsync<List<HourlyData>>(endpoint);

                if (data != null)
                {
                    _cache.Set(cacheKey, data, TimeSpan.FromHours(1));
                }

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting hourly breakdown: {ex.Message}");
                return null;
            }
        }

        public async Task<byte[]?> ExportAnalyticsAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var endpoint = $"analytics/export?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";
                // This would need a different implementation for file downloads
                _logger.LogInformation($"Exporting analytics from {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error exporting analytics: {ex.Message}");
                return null;
            }
        }
    }
}
