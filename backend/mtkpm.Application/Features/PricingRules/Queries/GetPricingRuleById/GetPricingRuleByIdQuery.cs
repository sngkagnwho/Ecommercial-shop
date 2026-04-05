using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.Pricing;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.PricingRules.Queries.GetPricingRuleById
{
    public class GetPricingRuleByIdQuery : IRequest<PricingRuleDto?>
    {
        public int Id { get; set; }

        public GetPricingRuleByIdQuery(int id)
        {
            Id = id;
        }
    }

    public class GetPricingRuleByIdQueryHandler : IRequestHandler<GetPricingRuleByIdQuery, PricingRuleDto?>
    {
        private readonly IPricingRuleRepository _repository;
        private readonly IMapper _mapper;

        public GetPricingRuleByIdQueryHandler(IPricingRuleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PricingRuleDto?> Handle(GetPricingRuleByIdQuery request, CancellationToken cancellationToken)
        {
            var rule = await _repository.GetByIdAsync(request.Id);
            return rule == null ? null : _mapper.Map<PricingRuleDto>(rule);
        }
    }
}
