namespace mtkpm.Admin.Features.Reports.Models
{
    /// <summary>
    /// Sales report model
    /// </summary>
    public class SalesReport
    {
        public DateTime GeneratedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public decimal TotalRevenue { get; set; }
        
        public decimal AvgOrderValue { get; set; }
        public decimal AvgProductPrice { get; set; }
        
        public List<SalesItem> SalesByProduct { get; set; } = new();
        public List<MonthlySales> MonthlySales { get; set; } = new();
    }

    /// <summary>
    /// Sales item
    /// </summary>
    public class SalesItem
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public double GrowthPercentage { get; set; }
    }

    /// <summary>
    /// Monthly sales data
    /// </summary>
    public class MonthlySales
    {
        public string? Month { get; set; }
        public int Orders { get; set; }
        public decimal Revenue { get; set; }
        public int Customers { get; set; }
    }

    /// <summary>
    /// Inventory report
    /// </summary>
    public class InventoryReport
    {
        public DateTime GeneratedDate { get; set; }
        public int TotalProducts { get; set; }
        public int LowStockProducts { get; set; }
        public int OutOfStockProducts { get; set; }
        public List<StockItem> StockDetails { get; set; } = new();
    }

    /// <summary>
    /// Stock item
    /// </summary>
    public class StockItem
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int CurrentStock { get; set; }
        public int MinimumStock { get; set; }
        public string? Status { get; set; } // InStock, LowStock, OutOfStock
    }
}
