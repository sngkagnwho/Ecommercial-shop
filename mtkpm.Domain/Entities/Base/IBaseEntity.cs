using System;

namespace mtkpm.Domain.Entities.Base
{
    public interface IBaseEntity
    {
        int Id { get; set; }
        DateTime CreateAt { get; set; }
        DateTime? UpdateAt { get; set; }
        int? CreatedBy { get; set; }
        int? UpdatedBy { get; set; }
        void SetCreated(int? createBy);
        void SetUpdated(int? updateBy);
    }
}
