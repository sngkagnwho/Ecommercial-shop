using FluentValidation;

namespace mtkpm.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Tên người dùng là bắt buộc")
                .Length(3, 100).WithMessage("Tên người dùng phải từ 3 đến 100 ký tự");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email là bắt buộc")
                .EmailAddress().WithMessage("Email không hợp lệ");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu là bắt buộc")
                .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Mật khẩu xác nhận không khớp");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^(\+84|0)[0-9]{9,10}$").WithMessage("Số điện thoại không hợp lệ")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
        }
    }
}
