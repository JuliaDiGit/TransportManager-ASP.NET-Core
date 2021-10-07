using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Models.Validation.Attributes;

namespace Models
{
    [DriverModelValidation]
    public class DriverModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SoftDeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public string Name { get; set; }
        public List<VehicleModel> Vehicles { get; set; }
    }
}