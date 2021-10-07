using System;
using Enums;
using Models.Validation.Attributes;

namespace Models
{
    [UserModelValidation]
    public class UserRequestModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SoftDeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
