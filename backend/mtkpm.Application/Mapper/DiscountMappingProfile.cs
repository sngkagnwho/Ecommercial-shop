using AutoMapper;
using mtkpm.Application.Common.DTOs.Discount;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Application.Mapper
{
    public class DiscountMappingProfile : Profile
    {
        public DiscountMappingProfile()
        {
            CreateMap<Discount, DiscountDto>()
                .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.IsExpired))
                .ForMember(dest => dest.IsBudgetExhausted, opt => opt.MapFrom(src => src.IsBudgetExhausted))
                .ForMember(dest => dest.IsUsageLimitReached, opt => opt.MapFrom(src => src.IsUsageLimitReached))
                .ForMember(dest => dest.CanBeUsed, opt => opt.MapFrom(src => src.CanBeUsed));

            CreateMap<CreateDiscountDto, Discount>()
                .ForMember(dest => dest.UsedCount, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.BudgetUsed, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            CreateMap<UpdateDiscountDto, Discount>()
                .ForMember(dest => dest.Code, opt => opt.Ignore())
                .ForMember(dest => dest.CreateAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateAt, opt => opt.Ignore());

            CreateMap<DiscountUsageHistory, DiscountUsageHistoryDto>();
        }
    }
}
