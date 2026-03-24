using MediatR;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Users.Commands.RemoveFavourite
{
    public class RemoveFavouriteCommandHandler : IRequestHandler<RemoveFavouriteCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveFavouriteCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(RemoveFavouriteCommand request, CancellationToken cancellationToken)
        {
            var favourite = await _unitOfWork.FavouriteProducts.GetByUserAndProductAsync(request.UserId, request.ProductId, cancellationToken);
            if (favourite == null)
            {
                throw new KeyNotFoundException("Sản phẩm yêu thích không tìm thấy");
            }

            _unitOfWork.FavouriteProducts.Remove(favourite);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
