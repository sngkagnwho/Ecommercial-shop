using System.ComponentModel.DataAnnotations;

namespace mtkpm.Application.Common.DTOs.Category
{
    public class UpdateCategoryDto
    {
        [Required(ErrorMessage = "Tên danh m?c là b?t bu?c")]
        [StringLength(100, ErrorMessage = "Tên danh m?c không ???c v??t quá 100 ký t?")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Mô t? không ???c v??t quá 500 ký t?")]
        public string Description { get; set; }
    }
}
