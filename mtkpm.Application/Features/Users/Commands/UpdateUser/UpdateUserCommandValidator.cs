using FluentValidation;

namespace mtkpm.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("User ID không h?p l?");

            RuleFor(x => x.UserName)
                .Length(3, 100).WithMessage("Tên ng??i dùng ph?i t? 3 ??n 100 ký t?")
                .When(x => !string.IsNullOrEmpty(x.UserName));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Email không h?p l?")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^(\+84|0)[0-9]{9,10}$").WithMessage("S? ?i?n tho?i không h?p l?")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
        }
    }
}
