using System;
using mtkpm.Domain.Entities.Base;

namespace mtkpm.Domain.Entities.Business
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; private set; }
        public virtual Order? Order { get; set; }
        
        public int ProductId { get; private set; }
        public virtual Product? Product { get; set; }
        
        // Snapshot data t?i th?i ?i?m ??t hàng
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal PriceAtOrder { get; private set; }
        public decimal TotalPrice => Quantity * PriceAtOrder;
        
        protected OrderItem() { }
        
        public OrderItem(int orderId, int productId, string productName, int quantity, decimal priceAtOrder)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0");
            if (priceAtOrder < 0)
                throw new ArgumentException("Price cannot be negative");
            
            OrderId = orderId;
            ProductId = productId;
            ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
            Quantity = quantity;
            PriceAtOrder = priceAtOrder;
        }
        
        public void UpdateQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0");
            
            Quantity = quantity;
        }
    }
}
