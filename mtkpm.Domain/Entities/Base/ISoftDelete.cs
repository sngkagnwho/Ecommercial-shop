using System;

namespace mtkpm.Domain.Entities.Base
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
        int? DeletedBy { get; set; }
        DateTime? DeletedAt { get; set; }
        void SetDeleted(int? deletedBy);
        void UndoDelete();
    }
}
