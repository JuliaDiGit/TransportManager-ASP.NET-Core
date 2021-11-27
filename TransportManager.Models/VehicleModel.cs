using System;
using System.ComponentModel.DataAnnotations;
using TransportManager.Models.Validation.Attributes;

namespace TransportManager.Models
{
    [VehicleModelValidation]
    public class VehicleModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SoftDeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int CompanyId { get; set; }
        public int? DriverId { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string GovernmentNumber { get; set; }
    }
}