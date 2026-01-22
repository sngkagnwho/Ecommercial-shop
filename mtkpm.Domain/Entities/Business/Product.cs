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
        protected Product()
        {

        }
        public Product(string name, string description,decimal price, int stockquality,string imageurl)
        {
           Name = name;
           Description = description;
           Price = price;
           StockQuantity = stockquality;
            ImageUrl = imageurl;

        }
    }

    
}
