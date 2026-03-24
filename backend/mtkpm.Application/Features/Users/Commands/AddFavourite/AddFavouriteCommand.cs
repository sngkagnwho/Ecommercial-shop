using MediatR;
using mtkpm.Application.Common.DTOs.User;

namespace mtkpm.Application.Features.Users.Commands.AddFavourite
{
    public class AddFavouriteCommand : IRequest<FavouriteProductDto>
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
}
