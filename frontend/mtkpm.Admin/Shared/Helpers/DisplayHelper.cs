namespace mtkpm.Admin.Shared.Helpers
{
    /// <summary>
    /// Helper class for formatting and display utilities
    /// </summary>
    public static class DisplayHelper
    {
        /// <summary>
        /// Format currency value for Vietnamese display
        /// </summary>
        public static string FormatVietnamCurrency(decimal value)
        {
            return value.ToString("N0") + " VND";
        }

        /// <summary>
        /// Format date in Vietnamese format
        /// </summary>
        public static string FormatDate(DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// Format date and time in Vietnamese format
        /// </summary>
        public static string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy HH:mm:ss");
        }

        /// <summary>
        /// Get status badge CSS class
        /// </summary>
        public static string GetStatusBadgeClass(string status)
        {
            return status?.ToLower() switch
            {
                "completed" or "success" => "badge badge-success",
                "pending" or "processing" => "badge badge-warning",
                "cancelled" or "failed" => "badge badge-danger",
                _ => "badge badge-secondary"
            };
        }

        /// <summary>
        /// Get status display text
        /// </summary>
        public static string GetStatusDisplayText(string status)
        {
            return status?.ToLower() switch
            {
                "pending" => "Chờ xử lý",
                "processing" => "Đang xử lý",
                "completed" => "Hoàn thành",
                "cancelled" => "Hủy bỏ",
                "refunded" => "Hoàn tiền",
                "delivered" => "Đã giao",
                "shipped" => "Đã gửi",
                _ => status ?? "Không xác định"
            };
        }
    }
}
