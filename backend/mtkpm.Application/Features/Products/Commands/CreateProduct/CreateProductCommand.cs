using MediatR;
using mtkpm.Application.Common.DTOs.Product;

namespace mtkpm.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<ProductDto>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
    }
}
