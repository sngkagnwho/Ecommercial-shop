using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mtkpm.Domain.Entities.Base;

namespace mtkpm.Domain.Entities.Business
{
    public class Product : SoftDeleteEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? ImageUrl { get; set; }
        
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        
        public bool IsAvailable => StockQuantity > 0 && !IsDeleted;

        protected Product()
        {

        }
        
        public Product(string name, string description, decimal price, int stockQuantity, int categoryId, string? imageUrl = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            Price = price >= 0 ? price : throw new ArgumentException("Price cannot be negative");
            StockQuantity = stockQuantity >= 0 ? stockQuantity : throw new ArgumentException("Stock quantity cannot be negative");
            CategoryId = categoryId;
            ImageUrl = imageUrl;
        }

        public void UpdateStockQuantity(int quantity)
        {
            if(quantity < 0)
            {
                throw new ArgumentException("Stock quantity cannot be negative.");
            }
            StockQuantity = quantity;   
        }
        
        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice < 0)
            {
                throw new ArgumentException("Price cannot be negative.");
            }
            Price = newPrice;
        }
        
        public void DecreaseStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0");
            
            if (StockQuantity < quantity)
                throw new InvalidOperationException("Not enough stock available");
            
            StockQuantity -= quantity;
        }
        
        public void IncreaseStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0");
            
            StockQuantity += quantity;
        }
    }
}
