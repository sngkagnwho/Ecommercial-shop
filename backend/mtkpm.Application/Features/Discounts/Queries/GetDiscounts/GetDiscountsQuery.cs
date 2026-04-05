using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.Discount;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Discounts.Queries.GetDiscounts
{
    public class GetDiscountsQuery : IRequest<List<DiscountDto>>
    {
        public bool IncludeInactive { get; set; }
    }

    public class GetDiscountsQueryHandler : IRequestHandler<GetDiscountsQuery, List<DiscountDto>>
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;

        public GetDiscountsQueryHandler(IDiscountRepository discountRepository, IMapper mapper)
        {
            _discountRepository = discountRepository;
            _mapper = mapper;
        }

        public async Task<List<DiscountDto>> Handle(GetDiscountsQuery request, CancellationToken cancellationToken)
        {
            var discounts = request.IncludeInactive
                ? await _discountRepository.GetAllDiscountsAsync()
                : await _discountRepository.GetActiveDiscountsAsync();

            return _mapper.Map<List<DiscountDto>>(discounts);
        }
    }
}
