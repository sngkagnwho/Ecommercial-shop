using mtkpm.Admin.Models.Product;
using mtkpm.Admin.Models.Order;

namespace mtkpm.Admin.Models.Dashboard
{
    public class DashboardViewModel
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalProducts { get; set; }
        public int TotalUsers { get; set; }
        public List<OrderViewModel> RecentOrders { get; set; } = new();
        public List<ProductViewModel> LowStockProducts { get; set; } = new();
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
