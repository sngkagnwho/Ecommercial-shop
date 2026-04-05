using mtkpm.Admin.Features.Reports.Models;
using mtkpm.Admin.Infrastructure.Http;
using mtkpm.Admin.Infrastructure.Caching;

namespace mtkpm.Admin.Features.Reports.Services
{
    /// <summary>
    /// Implementation of reports service
    /// </summary>
    public class ReportService : IReportService
    {
        private readonly IHttpClientWrapper _httpClient;
        private readonly ICacheService _cache;
        private readonly ILogger<ReportService> _logger;

        public ReportService(IHttpClientWrapper httpClient, ICacheService cache, ILogger<ReportService> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
        }

        public async Task<SalesReport?> GenerateSalesReportAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var cacheKey = $"sales_report_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}";
                var cached = _cache.Get<SalesReport>(cacheKey);
                if (cached != null)
                    return cached;

                var endpoint = $"reports/sales?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";
                var report = await _httpClient.GetAsync<SalesReport>(endpoint);

                if (report != null)
                {
                    _cache.Set(cacheKey, report, TimeSpan.FromHours(24));
                    _logger.LogInformation($"Sales report generated for {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");
                }

                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error generating sales report: {ex.Message}");
                return null;
            }
        }

        public async Task<InventoryReport?> GenerateInventoryReportAsync()
        {
            try
            {
                var cacheKey = "inventory_report";
                var cached = _cache.Get<InventoryReport>(cacheKey);
                if (cached != null)
                    return cached;

                var report = await _httpClient.GetAsync<InventoryReport>("reports/inventory");

                if (report != null)
                {
                    _cache.Set(cacheKey, report, TimeSpan.FromHours(6));
                    _logger.LogInformation("Inventory report generated");
                }

                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error generating inventory report: {ex.Message}");
                return null;
            }
        }

        public async Task<byte[]?> ExportSalesReportAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                _logger.LogInformation($"Exporting sales report from {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");
                // Implementation for PDF export would go here
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error exporting sales report: {ex.Message}");
                return null;
            }
        }

        public async Task<byte[]?> ExportInventoryReportAsync()
        {
            try
            {
                _logger.LogInformation("Exporting inventory report");
                // Implementation for PDF export would go here
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error exporting inventory report: {ex.Message}");
                return null;
            }
        }
    }
}
