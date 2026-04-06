using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Admin.Features.Dashboard.Models;
using mtkpm.Admin.Features.Dashboard.Services;
using mtkpm.Admin.Services;

namespace mtkpm.Admin.Features.Dashboard.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly ITokenManager _tokenManager;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            IDashboardService dashboardService,
            ITokenManager tokenManager,
            ILogger<DashboardController> logger)
        {
            _dashboardService = dashboardService;
            _tokenManager = tokenManager;
            _logger = logger;
        }

        /// <summary>
        /// Display main dashboard
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                if (!_tokenManager.IsTokenValid())
                {
                    TempData["ErrorMessage"] = "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.";
                    return RedirectToAction("Login", "Auth", new { returnUrl = Url.Action("Index", "Dashboard") });
                }

                var stats = await _dashboardService.GetDashboardStatsAsync();
                return View(stats ?? new DashboardStats());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading dashboard: {ex.Message}");
                TempData["Error"] = "Lỗi tải trang Dashboard";
                return View(new DashboardStats());
            }
        }

        /// <summary>
        /// Get JSON data for charts
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetChartData()
        {
            try
            {
                var stats = await _dashboardService.GetDashboardStatsAsync();
                return Json(new { success = true, data = stats });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting chart data: {ex.Message}");
                return Json(new { success = false, message = "Lỗi tải dữ liệu" });
            }
        }

        /// <summary>
        /// Get revenue chart data
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetRevenueChart(int days = 30)
        {
            try
            {
                var endDate = DateTime.Now;
                var startDate = endDate.AddDays(-days);
                var data = await _dashboardService.GetRevenueChartAsync(startDate, endDate);
                return Json(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting revenue chart: {ex.Message}");
                return Json(new { success = false, message = "Lỗi tải dữ liệu doanh thu" });
            }
        }

        /// <summary>
        /// Get order trends data
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetOrderTrends(int days = 30)
        {
            try
            {
                var trends = await _dashboardService.GetOrderTrendsAsync(days);
                return Json(new { success = true, data = trends });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting order trends: {ex.Message}");
                return Json(new { success = false, message = "Lỗi tải xu hướng đơn hàng" });
            }
        }
    }
}
