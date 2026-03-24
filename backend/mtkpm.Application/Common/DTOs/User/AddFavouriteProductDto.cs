using System.ComponentModel.DataAnnotations;

namespace mtkpm.Application.Common.DTOs.User
{
    public class AddFavouriteProductDto
    {
        [Required(ErrorMessage = "ID sản phẩm là bắt buộc")]
        public int ProductId { get; set; }
    }
}
