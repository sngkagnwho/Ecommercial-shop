using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtkpm.Domain.Entities.Base
{
    public abstract class SoftDeleteEntity : BaseEntity
    {
        public bool IsDeleted { get; set; } = false;
        public int? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public virtual void SetDeleted(int? deletedBy)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = deletedBy;
        }
        public virtual void UndoDelete()
        {
            IsDeleted = false;
            DeletedAt = null;
            DeletedBy = null;
        }
    }
}
