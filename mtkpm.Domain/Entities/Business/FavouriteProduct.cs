using mtkpm.Domain.Entities.Identity_Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mtkpm.Domain.Entities.Base;

namespace mtkpm.Domain.Entities.Business
{
    public class FavouriteProduct: SoftDeleteEntity
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreateAt { get; set; }
        public User? User { get; set; }
        public decimal UnitPrice { get; set; }
        public Product? Product { get; set; }
        public decimal TotalPrice => UnitPrice*Quantity;
        public FavouriteProduct()
        {
            
        }
        public FavouriteProduct(int userId, int productId)
        {
            UserId = userId;
            ProductId = productId;
            CreateAt = DateTime.UtcNow;
        }

    }
}
