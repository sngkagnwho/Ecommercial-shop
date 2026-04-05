namespace mtkpm.Admin.Shared.Helpers
{
    /// <summary>
    /// Helper class for pagination
    /// </summary>
    public static class PaginationHelper
    {
        /// <summary>
        /// Generate page numbers for pagination
        /// </summary>
        public static List<int> GetPageNumbers(int currentPage, int totalPages, int rangeSize = 5)
        {
            var pages = new List<int>();
            var startPage = Math.Max(1, currentPage - rangeSize / 2);
            var endPage = Math.Min(totalPages, startPage + rangeSize - 1);

            if (endPage - startPage < rangeSize - 1)
            {
                startPage = Math.Max(1, endPage - rangeSize + 1);
            }

            for (int i = startPage; i <= endPage; i++)
            {
                pages.Add(i);
            }

            return pages;
        }

        /// <summary>
        /// Calculate skip count for pagination
        /// </summary>
        public static int GetSkipCount(int pageIndex, int pageSize)
        {
            return (pageIndex - 1) * pageSize;
        }
    }
}
