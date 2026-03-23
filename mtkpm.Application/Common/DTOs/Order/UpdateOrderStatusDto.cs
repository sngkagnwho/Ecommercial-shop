using System.ComponentModel.DataAnnotations;
using mtkpm.Domain.Enums.Business;

namespace mtkpm.Application.Common.DTOs.Order
{
    public class UpdateOrderStatusDto
    {
        [Required(ErrorMessage = "Trạng thái đơn hàng là bắt buộc")]
        public OrderStatus Status { get; set; }
    }
}
