using FluentValidation;

namespace mtkpm.Application.Features.NotificationMethods.Commands.UnsubscribeNotificationMethod
{
    public class UnsubscribeNotificationMethodCommandValidator : AbstractValidator<UnsubscribeNotificationMethodCommand>
    {
        public UnsubscribeNotificationMethodCommandValidator()
        {
            RuleFor(x => x.MethodName)
                .NotEmpty().WithMessage("Tên ph??ng th?c thông báo là b?t bu?c")
                .Must(BeValidMethodName).WithMessage("Ph??ng th?c thông báo không h?p l?");
        }

        private static bool BeValidMethodName(string methodName)
        {
            var allowed = new[] { "email", "emailnotification", "sms", "smsnotification", "push", "pushnotification" };
            return allowed.Contains(methodName.Trim().ToLowerInvariant());
        }
    }
}
