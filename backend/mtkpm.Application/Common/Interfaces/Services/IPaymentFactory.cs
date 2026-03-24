using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Enums.Business;

namespace mtkpm.Application.Common.Interfaces.Services
{
    /// <summary>
    /// Factory Interface - t?o payment methods khác nhau
    /// S? d?ng Factory Design Pattern
    /// </summary>
    public interface IPaymentFactory
    {
        /// <summary>
        /// T?o payment method d?a theo type
        /// </summary>
        IPaymentMethod CreatePaymentMethod(PaymentMethodType paymentType);

        /// <summary>
        /// Ki?m tra payment method có ???c h? tr? không
        /// </summary>
        bool IsPaymentMethodSupported(PaymentMethodType paymentType);
    }
}
