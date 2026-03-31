using MediatR;
using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork, ILoggerService logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(request.Id);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {request.Id} not found");
            }

            _unitOfWork.Products.Remove(product);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogWarning($"Xóa sản phẩm: ProductId={product.Id}, Tên={product.Name}", "ProductService");

            return true;
        }
    }
}
