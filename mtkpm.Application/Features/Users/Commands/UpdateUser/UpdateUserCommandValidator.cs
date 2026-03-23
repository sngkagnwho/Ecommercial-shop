using FluentValidation;

namespace mtkpm.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("User ID không hợp lệ");

            RuleFor(x => x.UserName)
                .Length(3, 100).WithMessage("Tên người dùng phải từ 3 đến 100 ký tự")
                .When(x => !string.IsNullOrEmpty(x.UserName));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Email không hợp lệ")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^(\+84|0)[0-9]{9,10}$").WithMessage("Số điện thoại không hợp lệ")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
        }
    }
}
