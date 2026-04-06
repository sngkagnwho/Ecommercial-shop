namespace mtkpm.Admin.Features.Dashboard.Models
{
    /// <summary>
    /// Dashboard statistics model
    /// </summary>
    public class DashboardStats
    {
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public int TotalUsers { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalCategories { get; set; }
        public int TotalNotifications { get; set; }
        public int TotalPaymentMethods { get; set; }
        public int ActivePaymentMethods { get; set; }
        public int TotalDiscounts { get; set; }
        public int ActiveDiscounts { get; set; }
        public int ActiveProducts { get; set; }
        public int LowStockProducts { get; set; }
        public int OutOfStockProducts { get; set; }
        public int PendingOrders { get; set; }
        public int ProcessingOrders { get; set; }
        public int ShippingOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int CancelledOrders { get; set; }
        public int UnpaidOrders { get; set; }
        public int LockedUsers { get; set; }
        public int NewUsersThisMonth { get; set; }
        public decimal AverageOrderValue { get; set; }
        
        public int OrdersToday { get; set; }
        public int OrdersWeek { get; set; }
        public int OrdersMonth { get; set; }
        
        public decimal RevenueToday { get; set; }
        public decimal RevenueWeek { get; set; }
        public decimal RevenueMonth { get; set; }

        public DateTime GeneratedAt { get; set; } = DateTime.Now;
        
        public List<OrderTrend> OrderTrends { get; set; } = new();
        public List<RevenueChartData> RevenueCharts { get; set; } = new();
        public List<TopProduct> TopProducts { get; set; } = new();
        public List<RecentOrderItem> RecentOrders { get; set; } = new();
        public List<LowStockProductItem> LowStockItems { get; set; } = new();
    }

    /// <summary>
    /// Order trend data for charts
    /// </summary>
    public class OrderTrend
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }

    /// <summary>
    /// Revenue chart data
    /// </summary>
    public class RevenueChartData
    {
        public string? Month { get; set; }
        public decimal Amount { get; set; }
    }

    /// <summary>
    /// Top product by sales
    /// </summary>
    public class TopProduct
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int SalesCount { get; set; }
        public decimal Revenue { get; set; }
    }

    public class RecentOrderItem
    {
        public int Id { get; set; }
        public string? OrderNumber { get; set; }
        public string? CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int Status { get; set; }
        public bool IsPaid { get; set; }
    }

    public class LowStockProductItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? CategoryName { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
    }
}
