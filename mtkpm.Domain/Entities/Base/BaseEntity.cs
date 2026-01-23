using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtkpm.Domain.Entities.Base
{
    public abstract class BaseEntity
    {
        public virtual int Id { get; set; }
        public virtual DateTime  CreateAt { get; set; }= DateTime.UtcNow;
        public virtual DateTime? UpdateAt { get; set; }
        public virtual int? CreatedBy { get; set; }
        public virtual int? UpdatedBy { get; set; }
        public virtual void SetCreated(int? CreateBy)
        {
            CreateAt = DateTime.UtcNow;
            CreatedBy = CreateBy;
        }
        public virtual void SetUpdated(int? UpdateBy)
        {
            UpdateAt = DateTime.UtcNow;
            UpdatedBy = UpdateBy;
        }

    }
}
