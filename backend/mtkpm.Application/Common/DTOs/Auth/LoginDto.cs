using System.ComponentModel.DataAnnotations;

namespace mtkpm.Application.Common.DTOs.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email ho?c tên ng??i dùng là b?t bu?c")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "M?t kh?u là b?t bu?c")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
