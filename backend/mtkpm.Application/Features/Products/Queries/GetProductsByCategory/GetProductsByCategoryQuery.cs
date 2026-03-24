using MediatR;
using mtkpm.Application.Common.DTOs.Product;

namespace mtkpm.Application.Features.Products.Queries.GetProductsByCategory
{
    public class GetProductsByCategoryQuery : IRequest<IEnumerable<ProductDto>>
    {
        public int CategoryId { get; set; }

        public GetProductsByCategoryQuery(int categoryId)
        {
            CategoryId = categoryId;
        }
    }
}
