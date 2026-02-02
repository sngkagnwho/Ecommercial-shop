using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.Cart;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Cart.Queries.GetUserCart
{
    public class GetUserCartQueryHandler : IRequestHandler<GetUserCartQuery, CartDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserCartQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CartDto> Handle(GetUserCartQuery request, CancellationToken cancellationToken)
        {
            var cartItems = await _unitOfWork.CartItems.GetByUserIdWithProductsAsync(request.UserId, cancellationToken);
            var cartItemDtos = _mapper.Map<List<CartItemDto>>(cartItems);

            var cart = new CartDto
            {
                UserId = request.UserId,
                Items = cartItemDtos,
                TotalItems = cartItemDtos.Sum(x => x.Quantity),
                TotalAmount = cartItemDtos.Sum(x => x.TotalPrice)
            };

            return cart;
        }
    }
}
