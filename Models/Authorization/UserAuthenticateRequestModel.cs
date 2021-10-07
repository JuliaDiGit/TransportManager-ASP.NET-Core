using System.ComponentModel.DataAnnotations;

namespace Models.Authorization
{
    public class UserAuthenticateRequestModel
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
