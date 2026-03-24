using AutoMapper;
using mtkpm.Application.Common.DTOs.Cart;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Application.Mapper
{
    public class CartMappingProfile : Profile
    {
        public CartMappingProfile()
        {
            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
                .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Product != null ? src.Product.Description : string.Empty))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product != null ? src.Product.Price : 0))
                .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.Product != null ? src.Product.ImageUrl : null))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Product != null ? src.Quantity * src.Product.Price : 0))
                .ForMember(dest => dest.AvailableStock, opt => opt.MapFrom(src => src.Product != null ? src.Product.StockQuantity : 0));
        }
    }
}
