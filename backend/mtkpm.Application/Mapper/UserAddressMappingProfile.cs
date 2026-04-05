using AutoMapper;
using mtkpm.Application.Common.DTOs.User;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Application.Mapper
{
    public class UserAddressMappingProfile : Profile
    {
        public UserAddressMappingProfile()
        {
            CreateMap<UserAddress, UserAddressDto>();
            CreateMap<CreateUserAddressDto, UserAddress>();
            CreateMap<UpdateUserAddressDto, UserAddress>();
        }
    }
}
