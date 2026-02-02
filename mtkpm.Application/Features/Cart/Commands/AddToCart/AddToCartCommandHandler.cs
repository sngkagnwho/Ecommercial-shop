using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.Cart;
using mtkpm.Application.Common.Interfaces.Repositories;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Application.Features.Cart.Commands.AddToCart
{
    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, CartItemDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddToCartCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CartItemDto> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {request.ProductId} not found");
            }

            if (product.StockQuantity < request.Quantity)
            {
                throw new InvalidOperationException($"Not enough stock. Available: {product.StockQuantity}");
            }

            var existingCartItem = await _unitOfWork.CartItems.GetByUserAndProductAsync(request.UserId, request.ProductId, cancellationToken);

            if (existingCartItem != null)
            {
                existingCartItem.IncreaseQuantity(request.Quantity);
                _unitOfWork.CartItems.Update(existingCartItem);
            }
            else
            {
                var cartItem = new CartItem(request.UserId, request.ProductId, request.Quantity);
                await _unitOfWork.CartItems.AddAsync(cartItem);
                existingCartItem = cartItem;
            }

            await _unitOfWork.SaveChangesAsync();

            var cartItemWithProduct = await _unitOfWork.CartItems.GetByIdWithProductAsync(existingCartItem.Id, cancellationToken);
            return _mapper.Map<CartItemDto>(cartItemWithProduct);
        }
    }
}
