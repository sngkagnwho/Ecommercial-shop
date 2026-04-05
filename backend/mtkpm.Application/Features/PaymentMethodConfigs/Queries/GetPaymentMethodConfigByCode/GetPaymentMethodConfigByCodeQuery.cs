using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.Payment;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.PaymentMethodConfigs.Queries.GetPaymentMethodConfigByCode
{
    public class GetPaymentMethodConfigByCodeQuery : IRequest<PaymentMethodConfigDto?>
    {
        public string Code { get; set; } = string.Empty;

        public GetPaymentMethodConfigByCodeQuery(string code)
        {
            Code = code;
        }
    }

    public class GetPaymentMethodConfigByCodeQueryHandler : IRequestHandler<GetPaymentMethodConfigByCodeQuery, PaymentMethodConfigDto?>
    {
        private readonly IPaymentMethodConfigRepository _repository;
        private readonly IMapper _mapper;

        public GetPaymentMethodConfigByCodeQueryHandler(IPaymentMethodConfigRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaymentMethodConfigDto?> Handle(GetPaymentMethodConfigByCodeQuery request, CancellationToken cancellationToken)
        {
            var method = await _repository.GetByCodeAsync(request.Code);
            return method == null ? null : _mapper.Map<PaymentMethodConfigDto>(method);
        }
    }
}
