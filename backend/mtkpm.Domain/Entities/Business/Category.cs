using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mtkpm.Domain.Entities.Base;

namespace mtkpm.Domain.Entities.Business
{
    public class Category: BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
        
        protected Category()
        {
        }
        
        public Category(string name, string description)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
        }
    }
}
