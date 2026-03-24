using MediatR;
using mtkpm.Application.Common.DTOs.Auth;
using mtkpm.Application.Common.Interfaces;

namespace mtkpm.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
    {
        private readonly IAuthService _authService;

        public RegisterCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var registerDto = new RegisterDto
            {
                UserName = request.UserName,
                Email = request.Email,
                Password = request.Password,
                ConfirmPassword = request.ConfirmPassword,
                PhoneNumber = request.PhoneNumber
            };

            return await _authService.RegisterAsync(registerDto, request.IpAddress, request.DeviceInfo);
        }
    }
}
