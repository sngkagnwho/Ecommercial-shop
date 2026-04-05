using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.Discount;
using mtkpm.Application.Common.Interfaces.Repositories;
using DiscountEntity = mtkpm.Domain.Entities.Business.Discount;

namespace mtkpm.Application.Features.Discounts.Commands.CreateDiscount
{
    public class CreateDiscountCommand : IRequest<DiscountDto>
    {
        public CreateDiscountDto Dto { get; set; } = new();
        public int? UserId { get; set; }
    }

    public class CreateDiscountCommandHandler : IRequestHandler<CreateDiscountCommand, DiscountDto>
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;

        public CreateDiscountCommandHandler(IDiscountRepository discountRepository, IMapper mapper)
        {
            _discountRepository = discountRepository;
            _mapper = mapper;
        }

        public async Task<DiscountDto> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
        {
            var exists = await _discountRepository.CodeExistsAsync(request.Dto.Code);
            if (exists)
            {
                throw new InvalidOperationException("Mã chi?t kh?u ?ã t?n t?i");
            }

            var entity = _mapper.Map<DiscountEntity>(request.Dto);
            entity.CreatedByUserId = request.UserId ?? 0;
            entity.SetCreated(request.UserId);

            var created = await _discountRepository.AddAsync(entity);
            return _mapper.Map<DiscountDto>(created);
        }
    }
}
