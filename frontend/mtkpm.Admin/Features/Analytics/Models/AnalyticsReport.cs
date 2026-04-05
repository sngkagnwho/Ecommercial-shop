namespace mtkpm.Admin.Features.Analytics.Models
{
    /// <summary>
    /// Analytics report model
    /// </summary>
    public class AnalyticsReport
    {
        public int Period { get; set; } // Days
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public int TotalOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int CancelledOrders { get; set; }
        
        public decimal TotalRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
        
        public int NewCustomers { get; set; }
        public int ReturningCustomers { get; set; }
        
        public AnalyticsMetrics Metrics { get; set; } = new();
        public List<HourlyData> HourlyBreakdown { get; set; } = new();
        public List<CategoryPerformance> CategoryPerformance { get; set; } = new();
    }

    /// <summary>
    /// Analytics metrics
    /// </summary>
    public class AnalyticsMetrics
    {
        public decimal ConversionRate { get; set; }
        public decimal CartAbandonmentRate { get; set; }
        public decimal CustomerRetentionRate { get; set; }
        public decimal AverageProductsPerOrder { get; set; }
    }

    /// <summary>
    /// Hourly breakdown data
    /// </summary>
    public class HourlyData
    {
        public int Hour { get; set; }
        public int Orders { get; set; }
        public decimal Revenue { get; set; }
    }

    /// <summary>
    /// Category performance data
    /// </summary>
    public class CategoryPerformance
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int ProductsSold { get; set; }
        public decimal Revenue { get; set; }
        public double PercentageOfTotal { get; set; }
    }
}
