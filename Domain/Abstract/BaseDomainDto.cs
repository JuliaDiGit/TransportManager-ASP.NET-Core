using System;

namespace Domain.Abstract
{
    public class BaseDomainDto : IBaseDomainDto
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SoftDeletedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
