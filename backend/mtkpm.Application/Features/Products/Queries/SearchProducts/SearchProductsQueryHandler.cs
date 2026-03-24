using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.Product;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Products.Queries.SearchProducts
{
    public class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SearchProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _unitOfWork.Products.SearchAsync(request.SearchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
    }
}
