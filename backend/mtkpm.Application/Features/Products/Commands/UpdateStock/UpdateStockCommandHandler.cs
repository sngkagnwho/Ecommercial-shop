using MediatR;
using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Products.Commands.UpdateStock
{
    public class UpdateStockCommandHandler : IRequestHandler<UpdateStockCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;

        public UpdateStockCommandHandler(IUnitOfWork unitOfWork, ILoggerService logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {request.ProductId} not found");
            }

            product.UpdateStockQuantity(request.Quantity);
            _logger.LogInfo($"C?p nh?t s? l??ng t?n kho: ProductId={product.Id}, S? l??ng m?i={request.Quantity}", "ProductService");
            
            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
