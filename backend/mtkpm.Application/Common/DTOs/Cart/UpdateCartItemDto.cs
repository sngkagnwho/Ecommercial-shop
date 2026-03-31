using System.ComponentModel.DataAnnotations;

namespace mtkpm.Application.Common.DTOs.Cart
{
    public class UpdateCartItemDto
    {
        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }
    }
}
