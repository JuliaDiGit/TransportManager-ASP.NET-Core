using TransportManager.Common.Enums;
using TransportManager.Domain.Abstract;

namespace TransportManager.Domain
{
    public class UserRequest : BaseDomainDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
