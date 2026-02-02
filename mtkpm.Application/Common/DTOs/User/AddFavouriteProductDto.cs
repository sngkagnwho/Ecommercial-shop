using System.ComponentModel.DataAnnotations;

namespace mtkpm.Application.Common.DTOs.User
{
    public class AddFavouriteProductDto
    {
        [Required(ErrorMessage = "ID s?n ph?m là b?t bu?c")]
        public int ProductId { get; set; }
    }
}
