using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mtkpm.Domain.Entities.Identity_Auth;

namespace mtkpm.Domain.Entities.Business
{
    public class CartItem
    {
        public int Id { get; set; }
        public int UserId { get; private set; }
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }
        public DateTime AddedAt { get; private set; }
        
        public virtual User? User { get; set; }
        public virtual Product? Product { get; set; }
        
        protected CartItem()
        {
        }
        
        public CartItem(int userId, int productId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0");
            
            UserId = userId;
            ProductId = productId;
            Quantity = quantity;
            AddedAt = DateTime.UtcNow;
        }
        
        public void UpdateQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0");
            
            Quantity = quantity;
        }
        
        public void IncreaseQuantity(int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than 0");
            
            Quantity += amount;
        }
        
        public void DecreaseQuantity(int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than 0");
            
            if (Quantity - amount < 1)
                throw new InvalidOperationException("Quantity cannot be less than 1");
            
            Quantity -= amount;
        }
    }
}
