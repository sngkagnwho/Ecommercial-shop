using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.Product;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(request.Id);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {request.Id} not found");
            }

            var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                throw new ArgumentException($"Category with ID {request.CategoryId} not found");
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.UpdatePrice(request.Price);
            product.UpdateStockQuantity(request.StockQuantity);
            product.ImageUrl = request.ImageUrl;
            product.CategoryId = request.CategoryId;

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();

            var productDto = _mapper.Map<ProductDto>(product);
            productDto.CategoryName = category.Name;

            return productDto;
        }
    }
}
