using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtkpm.Admin.Features.Reports.Services;

namespace mtkpm.Admin.Features.Reports.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ReportsController : Controller
    {
        private readonly IReportService _reportService;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(IReportService reportService, ILogger<ReportsController> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        /// <summary>
        /// Display reports page
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Display sales report
        /// </summary>
        public async Task<IActionResult> Sales(int days = 30)
        {
            try
            {
                var endDate = DateTime.Now;
                var startDate = endDate.AddDays(-days);
                var report = await _reportService.GenerateSalesReportAsync(startDate, endDate);
                return View(report);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading sales report: {ex.Message}");
                TempData["Error"] = "Lỗi tải báo cáo bán hàng";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Display inventory report
        /// </summary>
        public async Task<IActionResult> Inventory()
        {
            try
            {
                var report = await _reportService.GenerateInventoryReportAsync();
                return View(report);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading inventory report: {ex.Message}");
                TempData["Error"] = "Lỗi tải báo cáo hàng hóa";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Export sales report as PDF
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ExportSalesReport(int days = 30)
        {
            try
            {
                var endDate = DateTime.Now;
                var startDate = endDate.AddDays(-days);
                var pdfData = await _reportService.ExportSalesReportAsync(startDate, endDate);
                
                if (pdfData == null)
                {
                    TempData["Warning"] = "Không thể xuất báo cáo";
                    return RedirectToAction("Sales");
                }

                return File(pdfData, "application/pdf", $"sales_report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error exporting sales report: {ex.Message}");
                TempData["Error"] = "Lỗi xuất báo cáo bán hàng";
                return RedirectToAction("Sales");
            }
        }

        /// <summary>
        /// Export inventory report as PDF
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ExportInventoryReport()
        {
            try
            {
                var pdfData = await _reportService.ExportInventoryReportAsync();
                
                if (pdfData == null)
                {
                    TempData["Warning"] = "Không thể xuất báo cáo";
                    return RedirectToAction("Inventory");
                }

                return File(pdfData, "application/pdf", $"inventory_report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error exporting inventory report: {ex.Message}");
                TempData["Error"] = "Lỗi xuất báo cáo hàng hóa";
                return RedirectToAction("Inventory");
            }
        }
    }
}
