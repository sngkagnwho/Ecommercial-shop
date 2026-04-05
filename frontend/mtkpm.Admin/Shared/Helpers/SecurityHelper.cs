namespace mtkpm.Admin.Shared.Helpers
{
    /// <summary>
    /// Helper class for security utilities
    /// </summary>
    public static class SecurityHelper
    {
        /// <summary>
        /// Sanitize HTML input to prevent XSS attacks
        /// </summary>
        public static string SanitizeHtml(string? input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // Replace potentially dangerous characters
            return input
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&#39;");
        }

        /// <summary>
        /// Check if email is valid
        /// </summary>
        public static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Check if phone number is valid (Vietnamese format)
        /// </summary>
        public static bool IsValidPhoneNumber(string? phone)
        {
            if (string.IsNullOrEmpty(phone))
                return false;

            return System.Text.RegularExpressions.Regex.IsMatch(phone, @"^(0|\+84)[0-9]{8,9}$");
        }
    }
}
