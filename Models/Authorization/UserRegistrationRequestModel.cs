using System.ComponentModel.DataAnnotations;
using Models.Validation.Attributes;

namespace Models.Authorization
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
