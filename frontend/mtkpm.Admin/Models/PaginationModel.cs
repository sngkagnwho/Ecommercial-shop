namespace mtkpm.Admin.Models
{
    public class PaginationModel
    {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; } = 0;
        public int TotalPages => (TotalItems + PageSize - 1) / PageSize;
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
