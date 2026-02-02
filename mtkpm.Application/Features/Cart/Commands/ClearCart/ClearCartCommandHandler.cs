using MediatR;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Cart.Commands.ClearCart
{
    public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClearCartCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            var cartItems = await _unitOfWork.CartItems.GetByUserIdAsync(request.UserId, cancellationToken);
            
            foreach (var item in cartItems)
            {
                _unitOfWork.CartItems.Remove(item);
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
