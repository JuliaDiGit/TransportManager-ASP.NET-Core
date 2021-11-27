using System.Collections.Generic;
using TransportManager.Domain.Abstract;

namespace TransportManager.Domain
{
    public class Company : BaseDomainDto
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public List<Driver> Drivers { get; set; }
        public List<Vehicle> Vehicles { get; set; }
    }
}