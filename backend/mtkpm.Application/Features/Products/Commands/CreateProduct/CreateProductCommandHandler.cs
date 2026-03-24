using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.Product;
using mtkpm.Application.Common.Interfaces.Repositories;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                throw new ArgumentException($"Category with ID {request.CategoryId} not found");
            }

            var product = new Product(
                name: request.Name,
                description: request.Description,
                price: request.Price,
                stockQuantity: request.StockQuantity,
                categoryId: request.CategoryId,
                imageUrl: request.ImageUrl
            );

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            var productDto = _mapper.Map<ProductDto>(product);
            productDto.CategoryName = category.Name;

            return productDto;
        }
    }
}
