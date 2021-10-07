using System.Collections.Generic;
using Entities.Abstract;

namespace Entities
{
    public class CompanyEntity : BaseEntity
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public virtual ICollection<DriverEntity> Drivers { get; set; }
        public virtual ICollection<VehicleEntity> Vehicles { get; set; }
    }
}