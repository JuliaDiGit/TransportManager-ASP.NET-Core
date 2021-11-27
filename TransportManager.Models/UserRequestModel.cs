using System;
using System.ComponentModel.DataAnnotations;
using TransportManager.Common.Enums;
using TransportManager.Models.Validation.Attributes;

namespace TransportManager.Models
{
    [UserRequestModelValidation]
    public class UserRequestModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SoftDeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
