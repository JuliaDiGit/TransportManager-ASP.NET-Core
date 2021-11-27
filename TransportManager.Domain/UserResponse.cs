using TransportManager.Common.Enums;
using TransportManager.Domain.Abstract;

namespace TransportManager.Domain
{
    public class UserResponse : BaseDomainDto
    {
        public string Login { get; set; }
        public Role Role { get; set; }
    }
}
