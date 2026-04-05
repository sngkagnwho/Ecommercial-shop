using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Admin.Features.Analytics.Services;

namespace mtkpm.Admin.Features.Analytics.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class AnalyticsController : Controller
    {
        private readonly IAnalyticsService _analyticsService;
        private readonly ILogger<AnalyticsController> _logger;

        public AnalyticsController(IAnalyticsService analyticsService, ILogger<AnalyticsController> logger)
        {
            _analyticsService = analyticsService;
            _logger = logger;
        }

        /// <summary>
        /// Display analytics page
        /// </summary>
        public async Task<IActionResult> Index(int days = 30)
        {
            try
            {
                var endDate = DateTime.Now;
                var startDate = endDate.AddDays(-days);
                var report = await _analyticsService.GetAnalyticsReportAsync(startDate, endDate);
                return View(report);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading analytics: {ex.Message}");
                TempData["Error"] = "Lỗi tải trang Analytics";
                return View();
            }
        }

        /// <summary>
        /// Get category performance data
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCategoryPerformance(int days = 30)
        {
            try
            {
                var endDate = DateTime.Now;
                var startDate = endDate.AddDays(-days);
                var data = await _analyticsService.GetCategoryPerformanceAsync(startDate, endDate);
                return Json(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting category performance: {ex.Message}");
                return Json(new { success = false, message = "Lỗi tải dữ liệu danh mục" });
            }
        }

        /// <summary>
        /// Get hourly breakdown for a specific date
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetHourlyBreakdown(DateTime? date = null)
        {
            try
            {
                var queryDate = date ?? DateTime.Now;
                var data = await _analyticsService.GetHourlyBreakdownAsync(queryDate);
                return Json(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting hourly breakdown: {ex.Message}");
                return Json(new { success = false, message = "Lỗi tải dữ liệu theo giờ" });
            }
        }

        /// <summary>
        /// Export analytics as CSV
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Export(int days = 30)
        {
            try
            {
                var endDate = DateTime.Now;
                var startDate = endDate.AddDays(-days);
                var csvData = await _analyticsService.ExportAnalyticsAsync(startDate, endDate);
                
                if (csvData == null)
                {
                    TempData["Warning"] = "Không thể xuất dữ liệu";
                    return RedirectToAction("Index");
                }

                return File(csvData, "text/csv", $"analytics_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error exporting analytics: {ex.Message}");
                TempData["Error"] = "Lỗi xuất dữ liệu";
                return RedirectToAction("Index");
            }
        }
    }
}
