using Domain.Abstract;

namespace Domain
{
    public class Vehicle : BaseDomainDto
    {
        public Company Company { get; set; }
        public int CompanyId { get; set; }
        public Driver Driver { get; set; }
        public int? DriverId { get; set; }
        public string Model { get; set; }
        public string GovernmentNumber { get; set; }
    }
}