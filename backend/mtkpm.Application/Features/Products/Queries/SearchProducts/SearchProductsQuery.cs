using MediatR;
using mtkpm.Application.Common.DTOs.Product;

namespace mtkpm.Application.Features.Products.Queries.SearchProducts
{
    public class SearchProductsQuery : IRequest<IEnumerable<ProductDto>>
    {
        public string SearchTerm { get; set; }

        public SearchProductsQuery(string searchTerm)
        {
            SearchTerm = searchTerm;
        }
    }
}
