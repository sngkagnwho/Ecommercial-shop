using System.ComponentModel.DataAnnotations;

namespace mtkpm.Application.Common.DTOs.User
{
    public class UpdateUserDto
    {
        [StringLength(100, ErrorMessage = "Tên ng??i dùng không ???c v??t quá 100 ký t?")]
        public string? UserName { get; set; }

        [EmailAddress(ErrorMessage = "Email không h?p l?")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "S? ?i?n tho?i không h?p l?")]
        public string? PhoneNumber { get; set; }
    }
}
