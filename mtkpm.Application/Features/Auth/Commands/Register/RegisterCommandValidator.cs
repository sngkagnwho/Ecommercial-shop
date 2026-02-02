using FluentValidation;

namespace mtkpm.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Tên ng??i dùng là b?t bu?c")
                .Length(3, 100).WithMessage("Tên ng??i dùng ph?i t? 3 ??n 100 ký t?");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email là b?t bu?c")
                .EmailAddress().WithMessage("Email không h?p l?");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("M?t kh?u là b?t bu?c")
                .MinimumLength(6).WithMessage("M?t kh?u ph?i có ít nh?t 6 ký t?");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("M?t kh?u xác nh?n không kh?p");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^(\+84|0)[0-9]{9,10}$").WithMessage("S? ?i?n tho?i không h?p l?")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
        }
    }
}
