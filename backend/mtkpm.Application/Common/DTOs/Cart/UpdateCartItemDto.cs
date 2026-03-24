using System.ComponentModel.DataAnnotations;

namespace mtkpm.Application.Common.DTOs.Cart
{
    public class UpdateCartItemDto
    {
        [Required(ErrorMessage = "S? l??ng là b?t bu?c")]
        [Range(1, int.MaxValue, ErrorMessage = "S? l??ng ph?i l?n h?n 0")]
        public int Quantity { get; set; }
    }
}
