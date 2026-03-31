using FluentValidation;

namespace mtkpm.Application.Features.Orders.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
    {
        public UpdateOrderStatusCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("ID đơn hàng không hợp lệ");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Trạng thái đơn hàng không hợp lệ");
        }
    }
}
