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
        public int UserId { get; private set; }
        public int ProductId { get; private set; }
        public DateTime AddedAt { get; private set; }
        
        public virtual User? User { get; set; }
        public virtual Product? Product { get; set; }
        
        protected FavouriteProduct()
        {
            
        }
        
        public FavouriteProduct(int userId, int productId)
        {
            UserId = userId;
            ProductId = productId;
            AddedAt = DateTime.UtcNow;
        }
    }
}
