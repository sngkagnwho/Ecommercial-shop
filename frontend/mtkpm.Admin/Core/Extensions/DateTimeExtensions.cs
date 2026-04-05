namespace mtkpm.Admin.Core.Extensions
{
    /// <summary>
    /// Extension methods for DateTime
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Get relative time (e.g., "2 hours ago")
        /// </summary>
        public static string GetRelativeTime(this DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;

            if (timeSpan.TotalSeconds < 60)
                return "vừa xong";
            else if (timeSpan.TotalMinutes < 60)
                return $"{(int)timeSpan.TotalMinutes} phút trước";
            else if (timeSpan.TotalHours < 24)
                return $"{(int)timeSpan.TotalHours} giờ trước";
            else if (timeSpan.TotalDays < 7)
                return $"{(int)timeSpan.TotalDays} ngày trước";
            else if (timeSpan.TotalDays < 30)
                return $"{(int)timeSpan.TotalDays / 7} tuần trước";
            else if (timeSpan.TotalDays < 365)
                return $"{(int)timeSpan.TotalDays / 30} tháng trước";
            else
                return $"{(int)timeSpan.TotalDays / 365} năm trước";
        }

        /// <summary>
        /// Format date in Vietnamese
        /// </summary>
        public static string FormatVietnamese(this DateTime dateTime, string format = "dd/MM/yyyy HH:mm")
        {
            return dateTime.ToString(format, new System.Globalization.CultureInfo("vi-VN"));
        }

        /// <summary>
        /// Get start of day
        /// </summary>
        public static DateTime GetStartOfDay(this DateTime dateTime)
        {
            return dateTime.Date;
        }

        /// <summary>
        /// Get end of day
        /// </summary>
        public static DateTime GetEndOfDay(this DateTime dateTime)
        {
            return dateTime.Date.AddDays(1).AddSeconds(-1);
        }
    }
}
