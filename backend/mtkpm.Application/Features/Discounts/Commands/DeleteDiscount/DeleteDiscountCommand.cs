using MediatR;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Discounts.Commands.DeleteDiscount
{
    public class DeleteDiscountCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
    }

    public class DeleteDiscountCommandHandler : IRequestHandler<DeleteDiscountCommand, bool>
    {
        private readonly IDiscountRepository _discountRepository;

        public DeleteDiscountCommandHandler(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public async Task<bool> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
        {
            var existing = await _discountRepository.GetByIdAsync(request.Id);
            if (existing == null)
            {
                return false;
            }

            await _discountRepository.DeleteAsync(request.Id, request.UserId ?? 0);
            return true;
        }
    }
}
