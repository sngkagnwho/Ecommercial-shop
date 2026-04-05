using mtkpm.Admin.Features.Dashboard.Models;
using mtkpm.Admin.Infrastructure.Http;
using mtkpm.Admin.Infrastructure.Caching;
using mtkpm.Admin.Models;

namespace mtkpm.Admin.Features.Dashboard.Services
{
    /// <summary>
    /// Implementation of dashboard analytics service
    /// </summary>
    public class DashboardService : IDashboardService
    {
        private readonly IHttpClientWrapper _httpClient;
        private readonly ICacheService _cache;
        private readonly ILogger<DashboardService> _logger;
        private const string CACHE_KEY_STATS = "dashboard_stats";
        private const string CACHE_KEY_REVENUE = "dashboard_revenue";
        private const string CACHE_KEY_TRENDS = "dashboard_trends";

        public DashboardService(IHttpClientWrapper httpClient, ICacheService cache, ILogger<DashboardService> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
        }

        public async Task<DashboardStats?> GetDashboardStatsAsync()
        {
            // Try to get from cache first
            var cached = _cache.Get<DashboardStats>(CACHE_KEY_STATS);
            if (cached != null)
                return cached;

            try
            {
                var stats = await _httpClient.GetAsync<DashboardStats>("dashboard/stats");
                
                if (stats != null)
                {
                    // Cache for 5 minutes
                    _cache.Set(CACHE_KEY_STATS, stats, TimeSpan.FromMinutes(5));
                    _logger.LogInformation("Dashboard stats retrieved and cached");
                }

                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting dashboard stats: {ex.Message}");
                return null;
            }
        }

        public async Task<List<RevenueChartData>?> GetRevenueChartAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var cacheKey = $"{CACHE_KEY_REVENUE}_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}";
                var cached = _cache.Get<List<RevenueChartData>>(cacheKey);
                if (cached != null)
                    return cached;

                var endpoint = $"dashboard/revenue?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";
                var data = await _httpClient.GetAsync<List<RevenueChartData>>(endpoint);

                if (data != null)
                {
                    _cache.Set(cacheKey, data, TimeSpan.FromMinutes(10));
                }

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting revenue chart: {ex.Message}");
                return null;
            }
        }

        public async Task<List<OrderTrend>?> GetOrderTrendsAsync(int days = 30)
        {
            try
            {
                var cacheKey = $"{CACHE_KEY_TRENDS}_{days}";
                var cached = _cache.Get<List<OrderTrend>>(cacheKey);
                if (cached != null)
                    return cached;

                var endpoint = $"dashboard/order-trends?days={days}";
                var trends = await _httpClient.GetAsync<List<OrderTrend>>(endpoint);

                if (trends != null)
                {
                    _cache.Set(cacheKey, trends, TimeSpan.FromMinutes(10));
                }

                return trends;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting order trends: {ex.Message}");
                return null;
            }
        }

        public async Task<List<TopProduct>?> GetTopProductsAsync(int count = 10)
        {
            try
            {
                var cacheKey = $"dashboard_top_products_{count}";
                var cached = _cache.Get<List<TopProduct>>(cacheKey);
                if (cached != null)
                    return cached;

                var endpoint = $"dashboard/top-products?count={count}";
                var products = await _httpClient.GetAsync<List<TopProduct>>(endpoint);

                if (products != null)
                {
                    _cache.Set(cacheKey, products, TimeSpan.FromHours(1));
                }

                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting top products: {ex.Message}");
                return null;
            }
        }
    }
}
