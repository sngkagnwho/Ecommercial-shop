using MediatR;
using mtkpm.Application.Common.DTOs.Auth;
using mtkpm.Application.Common.Interfaces;

namespace mtkpm.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly IAuthService _authService;

        public LoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var loginDto = new LoginDto
            {
                UserNameOrEmail = request.UserNameOrEmail,
                Password = request.Password,
                RememberMe = request.RememberMe
            };

            return await _authService.LoginAsync(loginDto, request.IpAddress, request.DeviceInfo);
        }
    }
}
