using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.Payment;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.PaymentMethodConfigs.Queries.GetPaymentMethodConfigs
{
    public class GetPaymentMethodConfigsQuery : IRequest<List<PaymentMethodConfigDto>>
    {
    }

    public class GetPaymentMethodConfigsQueryHandler : IRequestHandler<GetPaymentMethodConfigsQuery, List<PaymentMethodConfigDto>>
    {
        private readonly IPaymentMethodConfigRepository _repository;
        private readonly IMapper _mapper;

        public GetPaymentMethodConfigsQueryHandler(IPaymentMethodConfigRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PaymentMethodConfigDto>> Handle(GetPaymentMethodConfigsQuery request, CancellationToken cancellationToken)
        {
            var methods = await _repository.GetActiveMethodsAsync();
            return _mapper.Map<List<PaymentMethodConfigDto>>(methods);
        }
    }
}
