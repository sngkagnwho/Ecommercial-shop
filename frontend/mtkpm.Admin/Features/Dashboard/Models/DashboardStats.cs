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
        public int TotalRevenue { get; set; }
        
        public int OrdersToday { get; set; }
        public int OrdersWeek { get; set; }
        public int OrdersMonth { get; set; }
        
        public decimal RevenueToday { get; set; }
        public decimal RevenueWeek { get; set; }
        public decimal RevenueMonth { get; set; }
        
        public List<OrderTrend> OrderTrends { get; set; } = new();
        public List<RevenueChartData> RevenueCharts { get; set; } = new();
        public List<TopProduct> TopProducts { get; set; } = new();
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
}
