using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.Payment;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.PaymentMethodConfigs.Commands.UpdatePaymentMethodConfig
{
    public class UpdatePaymentMethodConfigCommand : IRequest<PaymentMethodConfigDto?>
    {
        public int Id { get; set; }
        public UpdatePaymentMethodConfigDto Dto { get; set; } = new();
        public int? UserId { get; set; }
    }

    public class UpdatePaymentMethodConfigCommandHandler : IRequestHandler<UpdatePaymentMethodConfigCommand, PaymentMethodConfigDto?>
    {
        private readonly IPaymentMethodConfigRepository _repository;
        private readonly IMapper _mapper;

        public UpdatePaymentMethodConfigCommandHandler(IPaymentMethodConfigRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaymentMethodConfigDto?> Handle(UpdatePaymentMethodConfigCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Id);
            if (existing == null)
            {
                return null;
            }

            _mapper.Map(request.Dto, existing);
            existing.SetUpdated(request.UserId);

            var updated = await _repository.UpdateAsync(existing);
            return _mapper.Map<PaymentMethodConfigDto>(updated);
        }
    }
}
