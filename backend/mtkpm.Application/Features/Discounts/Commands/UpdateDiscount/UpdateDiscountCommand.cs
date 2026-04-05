using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.Discount;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Discounts.Commands.UpdateDiscount
{
    public class UpdateDiscountCommand : IRequest<DiscountDto?>
    {
        public int Id { get; set; }
        public UpdateDiscountDto Dto { get; set; } = new();
        public int? UserId { get; set; }
    }

    public class UpdateDiscountCommandHandler : IRequestHandler<UpdateDiscountCommand, DiscountDto?>
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;

        public UpdateDiscountCommandHandler(IDiscountRepository discountRepository, IMapper mapper)
        {
            _discountRepository = discountRepository;
            _mapper = mapper;
        }

        public async Task<DiscountDto?> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
        {
            var existing = await _discountRepository.GetByIdAsync(request.Id);
            if (existing == null)
            {
                return null;
            }

            _mapper.Map(request.Dto, existing);
            existing.SetUpdated(request.UserId);

            var updated = await _discountRepository.UpdateAsync(existing);
            return _mapper.Map<DiscountDto>(updated);
        }
    }
}
