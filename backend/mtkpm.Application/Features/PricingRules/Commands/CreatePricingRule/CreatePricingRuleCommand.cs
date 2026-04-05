using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.Pricing;
using mtkpm.Application.Common.Interfaces.Repositories;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Application.Features.PricingRules.Commands.CreatePricingRule
{
    public class CreatePricingRuleCommand : IRequest<PricingRuleDto>
    {
        public CreatePricingRuleDto Dto { get; set; } = new();
        public int? UserId { get; set; }
    }

    public class CreatePricingRuleCommandHandler : IRequestHandler<CreatePricingRuleCommand, PricingRuleDto>
    {
        private readonly IPricingRuleRepository _repository;
        private readonly IMapper _mapper;

        public CreatePricingRuleCommandHandler(IPricingRuleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PricingRuleDto> Handle(CreatePricingRuleCommand request, CancellationToken cancellationToken)
        {
            var exists = await _repository.NameExistsAsync(request.Dto.Name);
            if (exists)
            {
                throw new InvalidOperationException("Tên quy t?c ??nh giá ?ã t?n t?i");
            }

            var entity = _mapper.Map<PricingRule>(request.Dto);
            entity.CreatedByUserId = request.UserId ?? 0;
            entity.SetCreated(request.UserId);

            var created = await _repository.AddAsync(entity);
            return _mapper.Map<PricingRuleDto>(created);
        }
    }
}
