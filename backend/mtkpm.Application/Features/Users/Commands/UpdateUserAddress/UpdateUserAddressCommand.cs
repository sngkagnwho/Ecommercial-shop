using MediatR;
using mtkpm.Application.Common.DTOs.User;

namespace mtkpm.Application.Features.Users.Commands.UpdateUserAddress
{
    public class UpdateUserAddressCommand : IRequest<UserAddressDto>
    {
        public int AddressId { get; set; }
        public int UserId { get; set; }
        public string ReceiverName { get; set; }
        public string PhoneNumber { get; set; }
        public string Street { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Label { get; set; }
        public bool IsDefault { get; set; }
    }
}
