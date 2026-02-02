using FluentValidation;

namespace mtkpm.Application.Features.Orders.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
    {
        public UpdateOrderStatusCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("ID ??n hàng không h?p l?");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Tr?ng thái ??n hàng không h?p l?");
        }
    }
}
