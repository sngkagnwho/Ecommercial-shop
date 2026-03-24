using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.Category;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(request.Id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {request.Id} not found");
            }

            category.Name = request.Name;
            category.Description = request.Description;

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(category);
        }
    }
}
