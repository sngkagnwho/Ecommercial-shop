using mtkpm.Admin.Features.Reports.Models;

namespace mtkpm.Admin.Features.Reports.Services
{
    /// <summary>
    /// Interface for reports service
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Generate sales report
        /// </summary>
        Task<SalesReport?> GenerateSalesReportAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Generate inventory report
        /// </summary>
        Task<InventoryReport?> GenerateInventoryReportAsync();

        /// <summary>
        /// Export sales report as PDF
        /// </summary>
        Task<byte[]?> ExportSalesReportAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Export inventory report as PDF
        /// </summary>
        Task<byte[]?> ExportInventoryReportAsync();
    }
}
