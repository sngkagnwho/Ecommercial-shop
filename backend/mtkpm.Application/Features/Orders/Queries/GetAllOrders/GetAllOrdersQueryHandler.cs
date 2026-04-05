using AutoMapper;
using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.Order;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Orders.Queries.GetAllOrders
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<OrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllOrdersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _unitOfWork.Orders.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }
    }
}
