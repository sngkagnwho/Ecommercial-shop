using MediatR;

namespace mtkpm.Application.Features.Auth.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
