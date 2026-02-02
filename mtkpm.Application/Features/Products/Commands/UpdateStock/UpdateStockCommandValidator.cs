using FluentValidation;

namespace mtkpm.Application.Features.Products.Commands.UpdateStock
{
    public class UpdateStockCommandValidator : AbstractValidator<UpdateStockCommand>
    {
        public UpdateStockCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ID s?n ph?m không h?p l?");

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("S? l??ng t?n kho ph?i l?n h?n ho?c b?ng 0");
        }
    }
}
