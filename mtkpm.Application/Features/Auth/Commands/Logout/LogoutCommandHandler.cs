using MediatR;
using mtkpm.Application.Common.Interfaces;

namespace mtkpm.Application.Features.Auth.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
    {
        private readonly IAuthService _authService;

        public LogoutCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            return await _authService.LogoutAsync(request.UserId, request.RefreshToken);
        }
    }
}
