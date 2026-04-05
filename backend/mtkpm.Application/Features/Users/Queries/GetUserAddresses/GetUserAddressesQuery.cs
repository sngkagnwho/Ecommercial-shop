using MediatR;
using mtkpm.Application.Common.DTOs.User;

namespace mtkpm.Application.Features.Users.Queries.GetUserAddresses
{
    public class GetUserAddressesQuery : IRequest<IEnumerable<UserAddressDto>>
    {
        public int UserId { get; set; }

        public GetUserAddressesQuery(int userId)
        {
            UserId = userId;
        }
    }
}
