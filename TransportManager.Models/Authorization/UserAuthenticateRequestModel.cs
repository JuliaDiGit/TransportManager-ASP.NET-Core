using System.ComponentModel.DataAnnotations;

namespace TransportManager.Models.Authorization
{
    public class UserAuthenticateRequestModel
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
