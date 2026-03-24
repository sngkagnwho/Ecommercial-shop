using MediatR;
using mtkpm.Application.Common.DTOs.Common;
using mtkpm.Application.Common.DTOs.Product;

namespace mtkpm.Application.Features.Products.Queries.GetProductsPaginated
{
    public class GetProductsPaginatedQuery : IRequest<PaginatedListDto<ProductDto>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int? CategoryId { get; set; }
        public string? SearchTerm { get; set; }
    }
}
