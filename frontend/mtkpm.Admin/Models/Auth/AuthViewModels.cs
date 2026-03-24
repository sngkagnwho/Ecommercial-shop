using System.Text.Json.Serialization;

namespace mtkpm.Admin.Models.Auth
{
    public class LoginRequest
    {
        [JsonPropertyName("userNameOrEmail")]
        public string UserNameOrEmail { get; set; } = "";
        
        [JsonPropertyName("password")]
        public string Password { get; set; } = "";
        
        [JsonPropertyName("rememberMe")]
        public bool RememberMe { get; set; } = true;
    }

    /// <summary>
    /// Wrapper model to match API response structure
    /// </summary>
    public class LoginApiResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        
        [JsonPropertyName("message")]
        public string Message { get; set; } = "";
        
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; } = "";
        
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; } = "";
        
        [JsonPropertyName("expiresAt")]
        public DateTime ExpiresAt { get; set; }
        
        [JsonPropertyName("user")]
        public UserInfo User { get; set; } = new();
        
        [JsonPropertyName("errors")]
        public List<string>? Errors { get; set; }
    }

    public class UserInfo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("userName")]
        public string UserName { get; set; } = "";
        
        [JsonPropertyName("email")]
        public string Email { get; set; } = "";
        
        [JsonPropertyName("phoneNumber")]
        public string? PhoneNumber { get; set; }
        
        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; } = new();
    }

    /// <summary>
    /// Converted model for internal use
    /// </summary>
    public class LoginResponse
    {
        public int UserId { get; set; }
        public string Email { get; set; } = "";
        public string UserName { get; set; } = "";
        public string FullName { get; set; } = "";
        public List<string> Roles { get; set; } = new();
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public int ExpiresIn { get; set; }
    }

    public class AdminUserViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string FullName { get; set; } = "";
        public string? PhoneNumber { get; set; }
        public List<string> Roles { get; set; } = new();
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
    }

    public class CreateAdminUserRequest
    {
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string FullName { get; set; } = "";
        public string? PhoneNumber { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
