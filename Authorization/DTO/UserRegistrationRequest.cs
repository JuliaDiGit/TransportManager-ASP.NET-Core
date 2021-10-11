using Enums;

namespace Authorization.DTO
{
    public class UserRegistrationRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
