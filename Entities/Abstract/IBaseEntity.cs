using System;

namespace Entities.Abstract
{
    public interface IBaseEntity
    {
        int Id { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime? SoftDeletedDate { get; set; }
        bool IsDeleted { get; set; }
    }
}