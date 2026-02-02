using MediatR;
using mtkpm.Application.Common.DTOs.Auth;
using mtkpm.Application.Common.Interfaces;

namespace mtkpm.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
    {
        private readonly IAuthService _authService;

        public RefreshTokenCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshTokenDto = new RefreshTokenDto
            {
                AccessToken = request.AccessToken,
                RefreshToken = request.RefreshToken
            };

            return await _authService.RefreshTokenAsync(refreshTokenDto, request.IpAddress, request.DeviceInfo);
        }
    }
}
