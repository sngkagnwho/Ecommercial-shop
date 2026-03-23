using MediatR;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(request.Id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Danh m?c có ID {request.Id} không t?n t?i");
            }

            var productsInCategory = await _unitOfWork.Products.GetByCategoryIdAsync(request.Id, cancellationToken);
            if (productsInCategory.Any())
            {
                throw new InvalidOperationException("Không th? xóa danh m?c có s?n ph?m. Vui lòng xóa ho?c di chuy?n s?n ph?m tr??c.");
            }

            _unitOfWork.Categories.Remove(category);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
