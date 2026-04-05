using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.Pricing;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.PricingRules.Commands.UpdatePricingRule
{
    public class UpdatePricingRuleCommand : IRequest<PricingRuleDto?>
    {
        public int Id { get; set; }
        public UpdatePricingRuleDto Dto { get; set; } = new();
        public int? UserId { get; set; }
    }

    public class UpdatePricingRuleCommandHandler : IRequestHandler<UpdatePricingRuleCommand, PricingRuleDto?>
    {
        private readonly IPricingRuleRepository _repository;
        private readonly IMapper _mapper;

        public UpdatePricingRuleCommandHandler(IPricingRuleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PricingRuleDto?> Handle(UpdatePricingRuleCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Id);
            if (existing == null)
            {
                return null;
            }

            var nameExists = await _repository.NameExistsAsync(request.Dto.Name, request.Id);
            if (nameExists)
            {
                throw new InvalidOperationException("Tên quy t?c ??nh giá ?ã t?n t?i");
            }

            _mapper.Map(request.Dto, existing);
            existing.SetUpdated(request.UserId);

            var updated = await _repository.UpdateAsync(existing);
            return _mapper.Map<PricingRuleDto>(updated);
        }
    }
}
