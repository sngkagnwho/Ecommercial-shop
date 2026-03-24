using MediatR;
using mtkpm.Application.Common.DTOs.Product;

namespace mtkpm.Application.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>
    {
    }
}
