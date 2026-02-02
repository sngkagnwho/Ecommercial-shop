using MediatR;

namespace mtkpm.Application.Features.Auth.Commands.Logout
{
    public class LogoutCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public string RefreshToken { get; set; }

        public LogoutCommand(int userId, string refreshToken)
        {
            UserId = userId;
            RefreshToken = refreshToken;
        }
    }
}
