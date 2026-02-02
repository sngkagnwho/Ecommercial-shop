using System.ComponentModel.DataAnnotations;
using mtkpm.Domain.Enums.Business;

namespace mtkpm.Application.Common.DTOs.Order
{
    public class UpdateOrderStatusDto
    {
        [Required(ErrorMessage = "Tr?ng thái ??n hàng là b?t bu?c")]
        public OrderStatus Status { get; set; }
    }
}
