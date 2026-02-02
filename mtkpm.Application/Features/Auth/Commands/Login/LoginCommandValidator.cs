using FluentValidation;

namespace mtkpm.Application.Features.Auth.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.UserNameOrEmail)
                .NotEmpty().WithMessage("Email ho?c tên ng??i dùng là b?t bu?c");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("M?t kh?u là b?t bu?c");
        }
    }
}
