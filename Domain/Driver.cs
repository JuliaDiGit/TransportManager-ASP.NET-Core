using System.Collections.Generic;
using Domain.Abstract;

namespace Domain
{
    public class Driver : BaseDomainDto
    {
        public Company Company { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public List<Vehicle> Vehicles { get; set; }
    }
}