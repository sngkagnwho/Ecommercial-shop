using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.Cart;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Cart.Commands.UpdateCartItem
{
    public class UpdateCartItemCommandHandler : IRequestHandler<UpdateCartItemCommand, CartItemDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCartItemCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CartItemDto> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
        {
            var cartItem = await _unitOfWork.CartItems.GetByIdWithProductAsync(request.CartItemId, cancellationToken);
            if (cartItem == null)
            {
                throw new KeyNotFoundException($"Cart item with ID {request.CartItemId} not found");
            }

            if (cartItem.UserId != request.UserId)
            {
                throw new UnauthorizedAccessException("You are not authorized to update this cart item");
            }

            if (cartItem.Product != null && cartItem.Product.StockQuantity < request.Quantity)
            {
                throw new InvalidOperationException($"Not enough stock. Available: {cartItem.Product.StockQuantity}");
            }

            cartItem.UpdateQuantity(request.Quantity);
            
            _unitOfWork.CartItems.Update(cartItem);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CartItemDto>(cartItem);
        }
    }
}
