using MediatR;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Cart.Commands.RemoveFromCart
{
    public class RemoveFromCartCommandHandler : IRequestHandler<RemoveFromCartCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveFromCartCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
        {
            var cartItem = await _unitOfWork.CartItems.GetByIdAsync(request.CartItemId);
            if (cartItem == null)
            {
                throw new KeyNotFoundException($"Cart item with ID {request.CartItemId} not found");
            }

            if (cartItem.UserId != request.UserId)
            {
                throw new UnauthorizedAccessException("You are not authorized to remove this cart item");
            }

            _unitOfWork.CartItems.Remove(cartItem);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
