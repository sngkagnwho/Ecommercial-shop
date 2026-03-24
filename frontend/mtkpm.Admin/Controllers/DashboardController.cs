using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Admin.Services;

namespace mtkpm.Admin.Controllers
{
    /// <summary>
    /// Admin dashboard controller for viewing key metrics and statistics
    /// </summary>
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            IOrderService orderService,
            IProductService productService,
            IUserService userService,
            ILogger<DashboardController> logger)
        {
            _orderService = orderService;
            _productService = productService;
            _userService = userService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var dashboardData = new Models.Dashboard.DashboardViewModel();

                // Get recent orders
                var ordersResult = await _orderService.GetOrdersAsync(1, 5);
                if (ordersResult != null)
                {
                    dashboardData.RecentOrders = ordersResult.Data;
                    dashboardData.TotalOrders = ordersResult.TotalCount;
                    dashboardData.TotalRevenue = ordersResult.Data.Sum(o => o.TotalAmount);
                }

                // Get products count
                var products = await _productService.GetAllProductsAsync();
                if (products != null)
                {
                    dashboardData.TotalProducts = products.Count;
                    dashboardData.LowStockProducts = products.Where(p => p.StockQuantity < 10).ToList();
                }

                // Get users count
                var usersResult = await _userService.GetUsersAsync(1, 1);
                if (usersResult != null)
                {
                    dashboardData.TotalUsers = usersResult.TotalCount;
                }

                return View(dashboardData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading dashboard: {ex.Message}");
                ViewBag.ErrorMessage = "Error loading dashboard data";
                return View(new Models.Dashboard.DashboardViewModel());
            }
        }
    }
}
