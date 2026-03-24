using AutoMapper;
using MediatR;
using mtkpm.Application.Common.DTOs.User;
using mtkpm.Application.Common.Interfaces.Repositories;

namespace mtkpm.Application.Features.Users.Queries.GetUserFavourites
{
    public class GetUserFavouritesQueryHandler : IRequestHandler<GetUserFavouritesQuery, IEnumerable<FavouriteProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserFavouritesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FavouriteProductDto>> Handle(GetUserFavouritesQuery request, CancellationToken cancellationToken)
        {
            var favourites = await _unitOfWork.FavouriteProducts.GetByUserIdWithProductsAsync(request.UserId, cancellationToken);
            return _mapper.Map<IEnumerable<FavouriteProductDto>>(favourites);
        }
    }
}
