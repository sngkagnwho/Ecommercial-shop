using System.ComponentModel.DataAnnotations;

namespace mtkpm.Domain.Enums.Business
{
    /// <summary>
    /// Payment Method Type Enum for Factory Pattern
    /// Replaces legacy PaymentMethod enum
    /// </summary>
    public enum PaymentMethodType
    {
        [Display(Name = "Thẻ tín dụng")]
        CreditCard = 1,

        [Display(Name = "Thẻ ghi nợ")]
        DebitCard = 2,

        [Display(Name = "Chuyển khoản ngân hàng")]
        BankTransfer = 3,

        [Display(Name = "PayPal")]
        PayPal = 4,

        [Display(Name = "Thanh toán khi nhận hàng")]
        COD = 5,

        [Display(Name = "Ví điện tử")]
        MobileWallet = 6
    }
}
