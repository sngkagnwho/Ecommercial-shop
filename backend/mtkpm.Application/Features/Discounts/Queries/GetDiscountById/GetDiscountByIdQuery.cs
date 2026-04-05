using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.Discount;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Discounts.Queries.GetDiscountById
{
    public class GetDiscountByIdQuery : IRequest<DiscountDto?>
    {
        public int Id { get; set; }

        public GetDiscountByIdQuery(int id)
        {
            Id = id;
        }
    }

    public class GetDiscountByIdQueryHandler : IRequestHandler<GetDiscountByIdQuery, DiscountDto?>
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;

        public GetDiscountByIdQueryHandler(IDiscountRepository discountRepository, IMapper mapper)
        {
            _discountRepository = discountRepository;
            _mapper = mapper;
        }

        public async Task<DiscountDto?> Handle(GetDiscountByIdQuery request, CancellationToken cancellationToken)
        {
            var discount = await _discountRepository.GetByIdAsync(request.Id);
            return discount == null ? null : _mapper.Map<DiscountDto>(discount);
        }
    }
}
