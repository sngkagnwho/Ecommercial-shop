using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.User;
using mtkpm.Application.Common.Interfaces.Repositories;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Application.Features.Users.Commands.AddFavourite
{
    public class AddFavouriteCommandHandler : IRequestHandler<AddFavouriteCommand, FavouriteProductDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddFavouriteCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<FavouriteProductDto> Handle(AddFavouriteCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {request.ProductId} not found");
            }

            var existingFavourite = await _unitOfWork.FavouriteProducts.GetByUserAndProductAsync(request.UserId, request.ProductId, cancellationToken);
            if (existingFavourite != null)
            {
                throw new InvalidOperationException("Product is already in favourites");
            }

            var favourite = new FavouriteProduct(request.UserId, request.ProductId);
            await _unitOfWork.FavouriteProducts.AddAsync(favourite);
            await _unitOfWork.SaveChangesAsync();

            var favouriteWithProduct = await _unitOfWork.FavouriteProducts.GetByIdWithProductAsync(favourite.Id, cancellationToken);
            return _mapper.Map<FavouriteProductDto>(favouriteWithProduct);
        }
    }
}
