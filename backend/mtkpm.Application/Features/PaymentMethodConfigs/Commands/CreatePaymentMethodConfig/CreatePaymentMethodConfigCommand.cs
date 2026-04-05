using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.Payment;
using mtkpm.Application.Common.Interfaces.Repositories;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Application.Features.PaymentMethodConfigs.Commands.CreatePaymentMethodConfig
{
    public class CreatePaymentMethodConfigCommand : IRequest<PaymentMethodConfigDto>
    {
        public CreatePaymentMethodConfigDto Dto { get; set; } = new();
        public int? UserId { get; set; }
    }

    public class CreatePaymentMethodConfigCommandHandler : IRequestHandler<CreatePaymentMethodConfigCommand, PaymentMethodConfigDto>
    {
        private readonly IPaymentMethodConfigRepository _repository;
        private readonly IMapper _mapper;

        public CreatePaymentMethodConfigCommandHandler(IPaymentMethodConfigRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaymentMethodConfigDto> Handle(CreatePaymentMethodConfigCommand request, CancellationToken cancellationToken)
        {
            var exists = await _repository.CodeExistsAsync(request.Dto.Code);
            if (exists)
            {
                throw new InvalidOperationException("Mã ph??ng th?c thanh toán ?ã t?n t?i");
            }

            var entity = _mapper.Map<PaymentMethodConfig>(request.Dto);
            entity.CreatedByUserId = request.UserId ?? 0;
            entity.SetCreated(request.UserId);

            var created = await _repository.AddAsync(entity);
            return _mapper.Map<PaymentMethodConfigDto>(created);
        }
    }
}
