using Enums;

namespace Domain.Authorization
{
    public class UserRegistrationRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
