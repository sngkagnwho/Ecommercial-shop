using mtkpm.Admin.Features.Dashboard.Models;
using mtkpm.Admin.Infrastructure.Caching;
using mtkpm.Admin.Models.Category;
using mtkpm.Admin.Models.Discount;
using mtkpm.Admin.Models.Notification;
using mtkpm.Admin.Models.Payment;
using mtkpm.Admin.Models.Product;
using mtkpm.Admin.Models.User;
using mtkpm.Admin.Services;

namespace mtkpm.Admin.Features.Dashboard.Services
{
    /// <summary>
    /// Implementation of dashboard analytics service
    /// </summary>
    public class DashboardService : IDashboardService
    {
        private readonly IAdminOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly ICategoryService _categoryService;
        private readonly IAdminDiscountService _discountService;
        private readonly IAdminPaymentService _paymentService;
        private readonly INotificationService _notificationService;
        private readonly ICacheService _cache;
        private readonly ILogger<DashboardService> _logger;
        private const string CACHE_KEY_STATS = "dashboard_stats";

        public DashboardService(
            IAdminOrderService orderService,
            IProductService productService,
            IUserService userService,
            ICategoryService categoryService,
            IAdminDiscountService discountService,
            IAdminPaymentService paymentService,
            INotificationService notificationService,
            ICacheService cache,
            ILogger<DashboardService> logger)
        {
            _orderService = orderService;
            _productService = productService;
            _userService = userService;
            _categoryService = categoryService;
            _discountService = discountService;
            _paymentService = paymentService;
            _notificationService = notificationService;
            _cache = cache;
            _logger = logger;
        }

        private async Task<T?> TryGetAsync<T>(Func<Task<T>> action, string source)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Dashboard source '{source}' failed: {ex.Message}");
                return default;
            }
        }

        public async Task<DashboardStats?> GetDashboardStatsAsync()
        {
            var cached = _cache.Get<DashboardStats>(CACHE_KEY_STATS);
            if (cached != null)
            {
                return cached;
            }

            try
            {
                var orders = await TryGetAsync(() => _orderService.GetAllOrdersAsync(), "orders") ?? new List<OrderViewModel>();
                var orderStats = await TryGetAsync(() => _orderService.GetOrderStatisticsAsync(), "order-stats") ?? new OrderStatisticsViewModel();
                var products = await TryGetAsync(() => _productService.GetAllProductsAsync(), "products") ?? new List<ProductViewModel>();
                var userStats = await TryGetAsync(() => _userService.GetUserStatisticsAsync(), "user-stats") ?? new UserStatisticsViewModel();
                var categories = await TryGetAsync(() => _categoryService.GetAllCategoriesAsync(), "categories") ?? new List<CategoryViewModel>();
                var discounts = await TryGetAsync(() => _discountService.GetDiscountStatisticsAsync(), "discount-stats") ?? new DiscountStatisticsViewModel();
                var paymentMethods = await TryGetAsync(() => _paymentService.GetPaymentMethodsAsync(), "payment-methods") ?? new List<PaymentMethodViewModel>();
                var notifications = await TryGetAsync(() => _notificationService.GetNotificationsAsync(), "notifications") ?? new List<NotificationViewModel>();

                var now = DateTime.Now;
                var today = now.Date;
                var weekStart = today.AddDays(-6);
                var monthStart = new DateTime(today.Year, today.Month, 1);

                var revenueSource = orders.Where(o => o.IsPaid).ToList();
                var orderItems = orders.SelectMany(o => o.OrderItems ?? new List<OrderItemViewModel>()).ToList();

                var topProducts = orderItems
                    .GroupBy(i => new { i.ProductId, i.ProductName })
                    .Select(g => new TopProduct
                    {
                        Id = g.Key.ProductId,
                        Name = g.Key.ProductName,
                        SalesCount = g.Sum(x => x.Quantity),
                        Revenue = g.Sum(x => x.TotalPrice)
                    })
                    .OrderByDescending(p => p.SalesCount)
                    .Take(5)
                    .ToList();

                var recentOrders = orders
                    .OrderByDescending(o => o.OrderDate)
                    .Take(8)
                    .Select(o => new RecentOrderItem
                    {
                        Id = o.Id,
                        OrderNumber = o.OrderNumber,
                        CustomerName = o.UserName,
                        OrderDate = o.OrderDate,
                        TotalAmount = o.TotalAmount,
                        Status = o.Status,
                        IsPaid = o.IsPaid
                    })
                    .ToList();

                var lowStockItems = products
                    .Where(p => p.StockQuantity <= 10)
                    .OrderBy(p => p.StockQuantity)
                    .Take(8)
                    .Select(p => new LowStockProductItem
                    {
                        Id = p.Id,
                        Name = p.Name,
                        CategoryName = p.CategoryName,
                        StockQuantity = p.StockQuantity,
                        Price = p.Price
                    })
                    .ToList();

                var orderTrends = Enumerable.Range(0, 14)
                    .Select(offset =>
                    {
                        var date = today.AddDays(-13 + offset);
                        return new OrderTrend
                        {
                            Date = date,
                            Count = orders.Count(o => o.OrderDate.Date == date)
                        };
                    })
                    .ToList();

                var revenueCharts = Enumerable.Range(0, 6)
                    .Select(offset =>
                    {
                        var month = monthStart.AddMonths(-5 + offset);
                        var nextMonth = month.AddMonths(1);
                        return new RevenueChartData
                        {
                            Month = month.ToString("MM/yyyy"),
                            Amount = revenueSource
                                .Where(o => o.OrderDate >= month && o.OrderDate < nextMonth)
                                .Sum(o => o.TotalAmount)
                        };
                    })
                    .ToList();

                var stats = new DashboardStats
                {
                    TotalOrders = orderStats.TotalOrders > 0 ? orderStats.TotalOrders : orders.Count,
                    TotalProducts = products.Count,
                    TotalUsers = userStats.TotalUsers,
                    TotalRevenue = orderStats.TotalRevenue > 0 ? orderStats.TotalRevenue : revenueSource.Sum(o => o.TotalAmount),
                    TotalCategories = categories.Count,
                    TotalNotifications = notifications.Count,
                    TotalPaymentMethods = paymentMethods.Count,
                    ActivePaymentMethods = paymentMethods.Count(p => p.IsActive),
                    TotalDiscounts = discounts.TotalDiscounts,
                    ActiveDiscounts = discounts.ActiveDiscounts,
                    ActiveProducts = products.Count(p => p.IsAvailable),
                    LowStockProducts = products.Count(p => p.StockQuantity > 0 && p.StockQuantity <= 10),
                    OutOfStockProducts = products.Count(p => p.StockQuantity <= 0),
                    PendingOrders = orderStats.PendingOrders,
                    ProcessingOrders = orders.Count(o => o.Status == 3),
                    ShippingOrders = orderStats.ShippingOrders,
                    CompletedOrders = orderStats.CompletedOrders,
                    CancelledOrders = orderStats.CancelledOrders,
                    UnpaidOrders = orders.Count(o => !o.IsPaid),
                    LockedUsers = userStats.LockedUsers,
                    NewUsersThisMonth = userStats.NewUsersThisMonth,
                    AverageOrderValue = orderStats.AverageOrderValue > 0
                        ? orderStats.AverageOrderValue
                        : (orders.Count > 0 ? orders.Average(o => o.TotalAmount) : 0),
                    OrdersToday = orders.Count(o => o.OrderDate.Date == today),
                    OrdersWeek = orders.Count(o => o.OrderDate.Date >= weekStart),
                    OrdersMonth = orders.Count(o => o.OrderDate.Date >= monthStart),
                    RevenueToday = revenueSource.Where(o => o.OrderDate.Date == today).Sum(o => o.TotalAmount),
                    RevenueWeek = revenueSource.Where(o => o.OrderDate.Date >= weekStart).Sum(o => o.TotalAmount),
                    RevenueMonth = revenueSource.Where(o => o.OrderDate.Date >= monthStart).Sum(o => o.TotalAmount),
                    GeneratedAt = DateTime.Now,
                    OrderTrends = orderTrends,
                    RevenueCharts = revenueCharts,
                    TopProducts = topProducts,
                    RecentOrders = recentOrders,
                    LowStockItems = lowStockItems
                };

                _cache.Set(CACHE_KEY_STATS, stats, TimeSpan.FromMinutes(2));
                _logger.LogInformation("Dashboard stats aggregated and cached");

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
            var stats = await GetDashboardStatsAsync();
            return stats?.RevenueCharts;
        }

        public async Task<List<OrderTrend>?> GetOrderTrendsAsync(int days = 30)
        {
            var stats = await GetDashboardStatsAsync();
            if (stats?.OrderTrends == null)
            {
                return null;
            }

            return stats.OrderTrends
                .OrderByDescending(t => t.Date)
                .Take(Math.Max(days, 1))
                .OrderBy(t => t.Date)
                .ToList();
        }

        public async Task<List<TopProduct>?> GetTopProductsAsync(int count = 10)
        {
            var stats = await GetDashboardStatsAsync();
            return stats?.TopProducts?.Take(Math.Max(count, 1)).ToList();
        }
    }
}
