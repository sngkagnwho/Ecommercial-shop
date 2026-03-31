using System.ComponentModel.DataAnnotations;

namespace mtkpm.Application.Common.DTOs.Auth
{
    public class RefreshTokenDto
    {
        [Required(ErrorMessage = "Access token là bắt buộc")]
        public string AccessToken { get; set; }

        [Required(ErrorMessage = "Refresh token là bắt buộc")]
        public string RefreshToken { get; set; }
    }
}
