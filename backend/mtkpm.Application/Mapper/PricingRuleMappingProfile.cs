using AutoMapper;
using mtkpm.Application.Common.DTOs.Pricing;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Application.Mapper
{
    public class PricingRuleMappingProfile : Profile
    {
        public PricingRuleMappingProfile()
        {
            CreateMap<PricingRule, PricingRuleDto>()
                .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.IsExpired))
                .ForMember(dest => dest.CanBeUsed, opt => opt.MapFrom(src => src.CanBeUsed));

            CreateMap<CreatePricingRuleDto, PricingRule>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            CreateMap<UpdatePricingRuleDto, PricingRule>()
                .ForMember(dest => dest.CreateAt, opt => opt.Ignore())
                .ForMember(dest => dest.RuleType, opt => opt.Ignore());
        }
    }
}
