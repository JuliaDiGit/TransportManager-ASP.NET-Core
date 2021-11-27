using TransportManager.Common.Enums;
using TransportManager.Entities.Abstract;

namespace TransportManager.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
