namespace mtkpm.Admin.Core.Extensions
{
    /// <summary>
    /// Extension methods for string
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Check if string is null or empty
        /// </summary>
        public static bool IsNullOrEmpty(this string? value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Check if string is null, empty or whitespace
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string? value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Truncate string to specified length
        /// </summary>
        public static string Truncate(this string value, int length, string suffix = "...")
        {
            if (value == null) return null;
            return value.Length > length ? value.Substring(0, length) + suffix : value;
        }

        /// <summary>
        /// Convert string to title case
        /// </summary>
        public static string ToTitleCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            var words = value.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 0)
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
            }
            return string.Join(" ", words);
        }

        /// <summary>
        /// Format currency
        /// </summary>
        public static string FormatCurrency(this decimal value, string? currency = "VND")
        {
            return value.ToString("N0") + " " + (currency ?? "VND");
        }
    }
}
