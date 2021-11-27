using System.ComponentModel.DataAnnotations;
using TransportManager.Models.Validation.Attributes;

namespace TransportManager.Models.Authorization
{
    [UserRegistrationRequestModelValidation]
    public class UserRegistrationRequestModel
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
