using System.ComponentModel.DataAnnotations;

namespace mtkpm.Application.Common.DTOs.Product
{
    public class UpdateProductDto
    {
        [Required(ErrorMessage = "Tên s?n ph?m là b?t bu?c")]
        [StringLength(200, ErrorMessage = "Tên s?n ph?m không ???c v??t quá 200 ký t?")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mô t? s?n ph?m là b?t bu?c")]
        [StringLength(1000, ErrorMessage = "Mô t? không ???c v??t quá 1000 ký t?")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Giá s?n ph?m là b?t bu?c")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá ph?i l?n h?n ho?c b?ng 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "S? l??ng t?n kho là b?t bu?c")]
        [Range(0, int.MaxValue, ErrorMessage = "S? l??ng t?n kho ph?i l?n h?n ho?c b?ng 0")]
        public int StockQuantity { get; set; }

        [Url(ErrorMessage = "URL hình ?nh không h?p l?")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Danh m?c là b?t bu?c")]
        public int CategoryId { get; set; }
    }
}
