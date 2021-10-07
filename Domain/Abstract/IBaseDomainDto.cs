using System;

namespace Domain.Abstract
{
    public interface IBaseDomainDto
    {
        int Id { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime? SoftDeletedDate { get; set; }
        bool IsDeleted { get; set; }
    }
}