using FluentValidation;

namespace mtkpm.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty().WithMessage("Access token là b?t bu?c");

            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token là b?t bu?c");
        }
    }
}
