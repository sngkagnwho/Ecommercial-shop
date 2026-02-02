using System.ComponentModel.DataAnnotations;

namespace mtkpm.Application.Common.DTOs.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Tên ng??i dùng là b?t bu?c")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tên ng??i dùng ph?i t? 3 ??n 100 ký t?")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email là b?t bu?c")]
        [EmailAddress(ErrorMessage = "Email không h?p l?")]
        public string Email { get; set; }

        [Required(ErrorMessage = "M?t kh?u là b?t bu?c")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "M?t kh?u ph?i t? 6 ??n 100 ký t?")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Xác nh?n m?t kh?u là b?t bu?c")]
        [Compare("Password", ErrorMessage = "M?t kh?u xác nh?n không kh?p")]
        public string ConfirmPassword { get; set; }

        [Phone(ErrorMessage = "S? ?i?n tho?i không h?p l?")]
        public string? PhoneNumber { get; set; }
    }
}
