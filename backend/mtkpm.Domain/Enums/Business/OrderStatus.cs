using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtkpm.Domain.Enums.Business
{
    public enum OrderStatus
    {
        [Display(Name = "Chờ xử lý")]
        Pending = 1,

        [Display(Name = "Đã xác nhận")]
        Confirmed = 2,

        [Display(Name = "Đang xử lý")]
        Processing = 3,

        [Display(Name = "Đang giao hàng")]
        Shipping = 4,

        [Display(Name = "Đã giao hàng")]
        Delivered = 5,

        [Display(Name = "Hoàn thành")]
        Completed = 6,

        [Display(Name = "Đã hủy")]
        Cancelled = 7,

        [Display(Name = "Hoàn trả")]
        Returned = 8,

        [Display(Name = "Thất bại")]
        Failed = 9
    }
}
