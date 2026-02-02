using System;

namespace mtkpm.Application.Common.DTOs.Common
{
    public class PaginatedListDto<T>
    {
        public List<T> Items { get; set; } = new();
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }

        public PaginatedListDto()
        {
        }

        public PaginatedListDto(List<T> items, int count, int pageIndex, int pageSize)
        {
            Items = items;
            TotalCount = count;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            HasPreviousPage = pageIndex > 1;
            HasNextPage = pageIndex < TotalPages;
        }
    }
}
