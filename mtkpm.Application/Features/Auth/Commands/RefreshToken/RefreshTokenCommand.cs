using MediatR;
using mtkpm.Application.Common.DTOs.Auth;

namespace mtkpm.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<AuthResponse>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string? IpAddress { get; set; }
        public string? DeviceInfo { get; set; }
    }
}
