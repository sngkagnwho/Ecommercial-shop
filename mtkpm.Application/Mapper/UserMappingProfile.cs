using AutoMapper;
using mtkpm.Application.Common.DTOs.User;
using mtkpm.Domain.Entities.Business;
using mtkpm.Domain.Entities.Identity_Auth;

namespace mtkpm.Application.Mapper
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreateAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdateAt));

            CreateMap<FavouriteProduct, FavouriteProductDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
                .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Product != null ? src.Product.Description : string.Empty))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product != null ? src.Product.Price : 0))
                .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.Product != null ? src.Product.ImageUrl : null))
                .ForMember(dest => dest.ProductStockQuantity, opt => opt.MapFrom(src => src.Product != null ? src.Product.StockQuantity : 0))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.Product != null && src.Product.IsAvailable));
        }
    }
}
