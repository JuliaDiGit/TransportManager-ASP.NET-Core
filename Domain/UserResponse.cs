using System;
using System.Collections.Generic;
using System.Text;
using Domain.Abstract;
using Enums;

namespace Domain
{
    public class UserResponse : BaseDomainDto
    {
        public string Login { get; set; }
        public Role Role { get; set; }
    }
}
