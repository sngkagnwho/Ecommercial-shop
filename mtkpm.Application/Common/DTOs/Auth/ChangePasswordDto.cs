using System.ComponentModel.DataAnnotations;

namespace mtkpm.Application.Common.DTOs.Auth
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "M?t kh?u hi?n t?i là b?t bu?c")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "M?t kh?u m?i là b?t bu?c")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "M?t kh?u ph?i t? 6 ??n 100 ký t?")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Xác nh?n m?t kh?u m?i là b?t bu?c")]
        [Compare("NewPassword", ErrorMessage = "M?t kh?u xác nh?n không kh?p")]
        public string ConfirmNewPassword { get; set; }
    }
}
