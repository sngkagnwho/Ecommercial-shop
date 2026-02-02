using FluentValidation;

namespace mtkpm.Application.Features.Auth.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID không h?p l?");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("M?t kh?u hi?n t?i là b?t bu?c");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("M?t kh?u m?i là b?t bu?c")
                .MinimumLength(6).WithMessage("M?t kh?u m?i ph?i có ít nh?t 6 ký t?")
                .NotEqual(x => x.CurrentPassword).WithMessage("M?t kh?u m?i ph?i khác m?t kh?u hi?n t?i");

            RuleFor(x => x.ConfirmNewPassword)
                .Equal(x => x.NewPassword).WithMessage("M?t kh?u xác nh?n không kh?p");
        }
    }
}
