using MediatR;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.PaymentMethodConfigs.Commands.DeletePaymentMethodConfig
{
    public class DeletePaymentMethodConfigCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
    }

    public class DeletePaymentMethodConfigCommandHandler : IRequestHandler<DeletePaymentMethodConfigCommand, bool>
    {
        private readonly IPaymentMethodConfigRepository _repository;

        public DeletePaymentMethodConfigCommandHandler(IPaymentMethodConfigRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeletePaymentMethodConfigCommand request, CancellationToken cancellationToken)
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
