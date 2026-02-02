using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtkpm.Domain.Enums.Business
{
    public enum PaymentMethod
    {
        [Display(Name = "Thanh toán khi nhận hàng")]
        CashOnDelivery = 1,

        [Display(Name = "Chuyển khoản ngân hàng")]
        BankTransfer = 2,

        [Display(Name = "Ví điện tử")]
        EWallet = 3,

        [Display(Name = "Thẻ tín dụng / ghi nợ")]
        CreditCard = 4
    }
}
