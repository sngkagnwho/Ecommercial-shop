using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mtkpm.Domain.Entities.Base;

namespace mtkpm.Domain.Entities.Identity_Auth
{
    public class RefeshToken : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User? User { get; set; }
        public string Token { get; private set; }

    }
}
