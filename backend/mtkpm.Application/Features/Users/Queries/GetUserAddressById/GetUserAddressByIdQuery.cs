using MediatR;
using mtkpm.Application.Common.DTOs.User;

namespace mtkpm.Application.Features.Users.Queries.GetUserAddressById
{
    public class GetUserAddressByIdQuery : IRequest<UserAddressDto?>
    {
        public int AddressId { get; set; }
        public int UserId { get; set; }

        public GetUserAddressByIdQuery(int addressId, int userId)
        {
            AddressId = addressId;
            UserId = userId;
        }
    }
}
