using System.ComponentModel.DataAnnotations;

namespace mtkpm.Application.Common.DTOs.User
{
    public class UpdateUserDto
    {
        [StringLength(100, ErrorMessage = "Tên người dùng không được vượt quá 100 ký tự")]
        public string? UserName { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? PhoneNumber { get; set; }
    }
}
