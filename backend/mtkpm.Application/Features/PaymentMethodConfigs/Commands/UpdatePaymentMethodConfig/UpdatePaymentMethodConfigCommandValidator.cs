using FluentValidation;

using FluentValidation;

namespace mtkpm.Application.Features.PaymentMethodConfigs.Commands.UpdatePaymentMethodConfig
{
    public class UpdatePaymentMethodConfigCommandValidator : AbstractValidator<UpdatePaymentMethodConfigCommand>
    {
        public UpdatePaymentMethodConfigCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id ph?i l?n h?n 0");

            RuleFor(x => x.Dto.Name)
                .NotEmpty().WithMessage("Tên ph??ng th?c thanh toán là b?t bu?c")
                .MaximumLength(100).WithMessage("Tên ph??ng th?c thanh toán không v??t quá 100 ký t?");

            RuleFor(x => x.Dto.ProcessingTime)
                .NotEmpty().WithMessage("Th?i gian x? lý là b?t bu?c")
                .MaximumLength(100).WithMessage("Th?i gian x? lý không v??t quá 100 ký t?");

            RuleFor(x => x.Dto.TransactionFeePercentage)
                .InclusiveBetween(0, 100).WithMessage("Phí giao d?ch ph?n tr?m ph?i trong kho?ng 0-100");

            RuleFor(x => x.Dto.MinAmount)
                .GreaterThanOrEqualTo(0).WithMessage("S? ti?n t?i thi?u ph?i >= 0");

            RuleFor(x => x.Dto.MaxAmount)
                .GreaterThanOrEqualTo(x => x.Dto.MinAmount).WithMessage("S? ti?n t?i ?a ph?i l?n h?n ho?c b?ng s? ti?n t?i thi?u");
        }
    }
}
