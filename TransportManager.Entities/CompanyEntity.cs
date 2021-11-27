using System.Collections.Generic;
using TransportManager.Entities.Abstract;

namespace TransportManager.Entities
{
    public class CompanyEntity : BaseEntity
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public virtual ICollection<DriverEntity> Drivers { get; set; }
        public virtual ICollection<VehicleEntity> Vehicles { get; set; }
    }
}