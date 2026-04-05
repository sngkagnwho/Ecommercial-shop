using MediatR;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.PricingRules.Commands.DeletePricingRule
{
    public class DeletePricingRuleCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
    }

    public class DeletePricingRuleCommandHandler : IRequestHandler<DeletePricingRuleCommand, bool>
    {
        private readonly IPricingRuleRepository _repository;

        public DeletePricingRuleCommandHandler(IPricingRuleRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeletePricingRuleCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Id);
            if (existing == null)
            {
                return false;
            }

            await _repository.DeleteAsync(request.Id, request.UserId ?? 0);
            return true;
        }
    }
}
