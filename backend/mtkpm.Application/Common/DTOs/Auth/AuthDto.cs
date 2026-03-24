using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtkpm.Application.Common.DTOs.Auth
{
    public class AuthDto
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }

    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public UserInfo? User { get; set; }
        public List<string>? Errors { get; set; }

        public static AuthResponse SuccessResult(string accessToken, string refreshToken, DateTime expiresAt, UserInfo user, string message = "Thành công")
        {
            return new AuthResponse
            {
                Success = true,
                Message = message,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = expiresAt,
                User = user
            };
        }

        public static AuthResponse FailureResult(string message, List<string>? errors = null)
        {
            return new AuthResponse
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }
    }

    public class UserInfo
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
