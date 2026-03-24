using MediatR;

namespace mtkpm.Application.Features.Users.Commands.RemoveFavourite
{
    public class RemoveFavouriteCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }

        public RemoveFavouriteCommand(int userId, int productId)
        {
            UserId = userId;
            ProductId = productId;
        }
    }
}
