using Entities.Abstract;
using Enums;

namespace Entities
{
    public class UserEntity : BaseEntity
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
