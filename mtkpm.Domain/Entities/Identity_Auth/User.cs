using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using mtkpm.Domain.Entities.Business;
namespace mtkpm.Domain.Entities.Identity_Auth
{
    public class User : IdentityUser<int>
    {
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public int? CreatedBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int? UpdatedBy { get; set; }

        public DateTime? DeleteAt { get; set; }
        public int? DeleteBy { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime? LastLoginAt { get; private set; }
        public virtual void SetCreated(int? createBy)
        {
            CreateAt = DateTime.UtcNow;
            CreatedBy = createBy;
        }
        public virtual void SetUpdated(int? updateBy)
        {
            UpdateAt = DateTime.UtcNow;
            UpdatedBy = updateBy;
        }
        public virtual void SetDeleted(int? deleteBy)
        {
            IsDeleted = true;
            DeleteAt = DateTime.UtcNow;
            DeleteBy = deleteBy;
        }
        public virtual void UndoDelete()
        {
            IsDeleted = false;
            DeleteBy = null;
            DeleteAt = null;
        }
    }
}
