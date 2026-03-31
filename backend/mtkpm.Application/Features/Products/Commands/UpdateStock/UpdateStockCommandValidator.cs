using FluentValidation;

namespace mtkpm.Application.Features.Products.Commands.UpdateStock
{
    public class UpdateStockCommandValidator : AbstractValidator<UpdateStockCommand>
    {
        public UpdateStockCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ID sản phẩm không hợp lệ");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Số lượng tồn kho phải lớn hơn hoặc bằng 0");
        }
    }
}
