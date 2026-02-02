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
                throw new KeyNotFoundException($"Category with ID {request.Id} not found");
            }

            var productsInCategory = await _unitOfWork.Products.GetByCategoryIdAsync(request.Id, cancellationToken);
            if (productsInCategory.Any())
            {
                throw new InvalidOperationException("Cannot delete category that has products. Please delete or move products first.");
            }

            _unitOfWork.Categories.Remove(category);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
