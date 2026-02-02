using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.Order;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Orders.GetWithDetailsAsync(request.Id, cancellationToken);
            
            if (order == null)
            {
                return null;
            }

            return _mapper.Map<OrderDto>(order);
        }
    }
}
