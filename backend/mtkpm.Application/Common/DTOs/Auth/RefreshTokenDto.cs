using System.ComponentModel.DataAnnotations;

namespace mtkpm.Application.Common.DTOs.Auth
{
    public class RefreshTokenDto
    {
        [Required(ErrorMessage = "Access token là b?t bu?c")]
        public string AccessToken { get; set; }

        [Required(ErrorMessage = "Refresh token là b?t bu?c")]
        public string RefreshToken { get; set; }
    }
}
