using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.Order;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Orders.Queries.GetOrderByNumber
{
    public class GetOrderByNumberQueryHandler : IRequestHandler<GetOrderByNumberQuery, OrderDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOrderByNumberQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OrderDto?> Handle(GetOrderByNumberQuery request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Orders.GetByOrderNumberAsync(request.OrderNumber, cancellationToken);
            
            if (order == null)
            {
                return null;
            }

            return _mapper.Map<OrderDto>(order);
        }
    }
}
