using MediatR;

namespace mtkpm.Application.Features.Users.Commands.DeleteUserAddress
{
    public class DeleteUserAddressCommand : IRequest<bool>
    {
        public int AddressId { get; set; }
        public int UserId { get; set; }
    }
}
