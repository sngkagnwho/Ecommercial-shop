using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.Pricing;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.PricingRules.Queries.GetPricingRules
{
    public class GetPricingRulesQuery : IRequest<List<PricingRuleDto>>
    {
    }

    public class GetPricingRulesQueryHandler : IRequestHandler<GetPricingRulesQuery, List<PricingRuleDto>>
    {
        private readonly IPricingRuleRepository _repository;
        private readonly IMapper _mapper;

        public GetPricingRulesQueryHandler(IPricingRuleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PricingRuleDto>> Handle(GetPricingRulesQuery request, CancellationToken cancellationToken)
        {
            var rules = await _repository.GetActiveRulesAsync();
            return _mapper.Map<List<PricingRuleDto>>(rules);
        }
    }
}
