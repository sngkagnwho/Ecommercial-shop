using MediatR;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Cart.Queries.GetCartItemCount
{
    public class GetCartItemCountQueryHandler : IRequestHandler<GetCartItemCountQuery, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCartItemCountQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(GetCartItemCountQuery request, CancellationToken cancellationToken)
        {
            var cartItems = await _unitOfWork.CartItems.GetByUserIdAsync(request.UserId, cancellationToken);
            return cartItems.Sum(x => x.Quantity);
        }
    }
}
