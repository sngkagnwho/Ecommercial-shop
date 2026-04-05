using AutoMapper;
using mtkpm.Application.Common.DTOs.Payment;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Application.Mapper
{
    public class PaymentMethodConfigMappingProfile : Profile
    {
        public PaymentMethodConfigMappingProfile()
        {
            CreateMap<PaymentMethodConfig, PaymentMethodConfigDto>();

            CreateMap<CreatePaymentMethodConfigDto, PaymentMethodConfig>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            CreateMap<UpdatePaymentMethodConfigDto, PaymentMethodConfig>()
                .ForMember(dest => dest.Code, opt => opt.Ignore())
                .ForMember(dest => dest.CreateAt, opt => opt.Ignore());
        }
    }
}
