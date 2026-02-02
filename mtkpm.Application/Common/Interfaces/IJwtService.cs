using System.Security.Claims;
using mtkpm.Domain.Entities.Identity_Auth;

namespace mtkpm.Application.Common.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user, IList<string> roles);
        string GenerateRefreshToken();
        ClaimsPrincipal? ValidateToken(string token, bool validateLifetime = true);
        int? GetUserIdFromExpiredToken(string token);
    }
}
