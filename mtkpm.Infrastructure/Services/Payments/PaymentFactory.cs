using mtkpm.Application.Common.Interfaces;
using mtkpm.Application.Common.Interfaces.Services;
using mtkpm.Domain.Events;
using mtkpm.Domain.Enums.Business;

namespace mtkpm.Infrastructure.Services.Payments
{
    /// <summary>
    /// Payment Factory Implementation - Factory Design Pattern
    /// T?o payment methods khác nhau d?a theo type
    /// </summary>
    public class PaymentFactory : IPaymentFactory
    {
        private readonly ILoggerService _logger;

        public PaymentFactory(ILoggerService logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// T?o payment method instance d?a theo type
        /// Factory Pattern: Encapsulate object creation logic
        /// </summary>
        public IPaymentMethod CreatePaymentMethod(PaymentMethodType paymentType)
        {
            _logger.LogInfo($"Creating payment method: {paymentType}", "PaymentFactory");

            return paymentType switch
            {
                PaymentMethodType.CreditCard => new CreditCardPaymentService(_logger),
                PaymentMethodType.DebitCard => new CreditCardPaymentService(_logger),
                PaymentMethodType.BankTransfer => new BankTransferPaymentService(_logger),
                PaymentMethodType.PayPal => new PayPalPaymentService(_logger),
                PaymentMethodType.COD => new CODPaymentService(_logger),
                PaymentMethodType.MobileWallet => new CreditCardPaymentService(_logger),
                _ => throw new NotSupportedException($"Payment method {paymentType} is not supported")
            };
        }

        /// <summary>
        /// Ki?m tra payment method có ???c h? tr? không
        /// </summary>
        public bool IsPaymentMethodSupported(PaymentMethodType paymentType)
        {
            return paymentType switch
            {
                PaymentMethodType.CreditCard or
                PaymentMethodType.DebitCard or
                PaymentMethodType.BankTransfer or
                PaymentMethodType.PayPal or
                PaymentMethodType.COD or
                PaymentMethodType.MobileWallet => true,
                _ => false
            };
        }
    }
}
