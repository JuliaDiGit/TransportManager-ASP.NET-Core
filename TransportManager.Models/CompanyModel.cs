using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TransportManager.Models.Validation.Attributes;

namespace TransportManager.Models
{
    [CompanyModelValidation]
    public class CompanyModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SoftDeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public string CompanyName { get; set; }
        public List<DriverModel> Drivers { get; set; } 
        public List<VehicleModel> Vehicles { get; set; }
    }
}