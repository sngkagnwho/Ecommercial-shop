using MediatR;
using mtkpm.Application.Common.DTOs.User;

namespace mtkpm.Application.Features.Users.Queries.GetUserFavourites
{
    public class GetUserFavouritesQuery : IRequest<IEnumerable<FavouriteProductDto>>
    {
        public int UserId { get; set; }

        public GetUserFavouritesQuery(int userId)
        {
            UserId = userId;
        }
    }
}
