using System;
using System.Collections.Generic;
using System.Text;
using Enums;

namespace Models
{
    public class UserResponseModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SoftDeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string Login { get; set; }
        public Role Role { get; set; }
    }
}
