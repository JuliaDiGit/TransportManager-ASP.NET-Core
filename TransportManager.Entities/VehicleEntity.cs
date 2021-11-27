using TransportManager.Entities.Abstract;

namespace TransportManager.Entities
{
    public class VehicleEntity : BaseEntity
    {
        public string Model { get; set; }
        public string GovernmentNumber { get; set; }
        
        public CompanyEntity Company { get; set; }
        public int CompanyId { get; set; }
        
        public DriverEntity Driver { get; set; }
        public int? DriverId { get; set; }

    }
}