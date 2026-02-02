using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.Category;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Categories.GetByIdWithProductsAsync(request.Id, cancellationToken);
            
            if (category == null)
            {
                return null;
            }

            return _mapper.Map<CategoryDto>(category);
        }
    }
}
