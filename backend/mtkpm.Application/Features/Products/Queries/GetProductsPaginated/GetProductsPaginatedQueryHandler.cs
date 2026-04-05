using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using mtkpm.Application.Common.DTOs.Common;
using mtkpm.Application.Common.DTOs.Product;
using mtkpm.Application.Common.Interfaces.Repositories;
using mtkpm.Application.Mapper;

namespace mtkpm.Application.Features.Products.Queries.GetProductsPaginated
{
    public class GetProductsPaginatedQueryHandler : IRequestHandler<GetProductsPaginatedQuery, PaginatedListDto<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductsPaginatedQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedListDto<ProductDto>> Handle(GetProductsPaginatedQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Domain.Entities.Business.Product> query = _unitOfWork.Products.GetAllQueryable()
                .Include(p => p.Category);

            if (request.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == request.CategoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(searchTerm) 
                                      || p.Description.ToLower().Contains(searchTerm));
            }

            query = query.Where(p => !p.IsDeleted);

            // Sort by newest first
            query = query.OrderByDescending(p => p.CreateAt);

            return await query.ToPaginatedListAsync<Domain.Entities.Business.Product, ProductDto>(
                request.PageIndex, 
                request.PageSize, 
                _mapper);
        }
    }
}
