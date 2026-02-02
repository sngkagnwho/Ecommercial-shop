using MediatR;

namespace mtkpm.Application.Features.Products.Commands.UpdateStock
{
    public class UpdateStockCommand : IRequest<bool>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public UpdateStockCommand(int productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
