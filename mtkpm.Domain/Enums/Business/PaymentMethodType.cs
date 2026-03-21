using System.ComponentModel.DataAnnotations;

namespace mtkpm.Domain.Enums.Business
{
    /// <summary>
    /// Payment Method Type Enum for Factory Pattern
    /// Replaces legacy PaymentMethod enum
    /// </summary>
    public enum PaymentMethodType
    {
        [Display(Name = "Th? tín d?ng")]
        CreditCard = 1,

        [Display(Name = "Th? ghi n?")]
        DebitCard = 2,

        [Display(Name = "Chuy?n kho?n ngân hàng")]
        BankTransfer = 3,

        [Display(Name = "PayPal")]
        PayPal = 4,

        [Display(Name = "Thanh toán khi nh?n hàng")]
        COD = 5,

        [Display(Name = "Ví ?i?n t? di ??ng")]
        MobileWallet = 6
    }
}
