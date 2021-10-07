using Domain.Abstract;
using Enums;

namespace Domain
{
    public class UserRequest : BaseDomainDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
