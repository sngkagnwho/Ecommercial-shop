using AutoMapper;
using mtkpm.Application.Common.DTOs.Order;
using mtkpm.Domain.Entities.Business;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace mtkpm.Application.Mapper
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null))
                .ForMember(dest => dest.StatusDisplay, opt => opt.MapFrom(src => GetEnumDisplayName(src.Status)))
                .ForMember(dest => dest.PaymentMethodDisplay, opt => opt.MapFrom(src => GetEnumDisplayName(src.PaymentMethod)))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreateAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdateAt));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice));
        }

        private static string GetEnumDisplayName(Enum enumValue)
        {
            var displayAttribute = enumValue.GetType()
                .GetField(enumValue.ToString())
                ?.GetCustomAttribute<DisplayAttribute>();
            
            return displayAttribute?.Name ?? enumValue.ToString();
        }
    }
}
