using System.ComponentModel.DataAnnotations;

namespace mtkpm.Application.Common.DTOs.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email hoặc tên người dùng là bắt buộc")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
