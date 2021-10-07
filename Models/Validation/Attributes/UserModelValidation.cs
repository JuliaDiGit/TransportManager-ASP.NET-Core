using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Models.Validation.Attributes
{
    public class UserModelValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            
            if (value is UserRequestModel userModel)
            {
                //до 15 знаков, не должно быть цифр и спецсимволов.
                string loginPattern = "^[a-zA-Z]{1,15}$";
                if (!Regex.IsMatch(userModel.Login, loginPattern))
                    return new ValidationResult(Resources.Error_IncorrectUserLogin);

                //2)до 20 символов, обязательна хотя бы 1 заглавная и 1 цифра, не должно быть спецсимволов.
                string pattern1 = "(?=.*[0-9])(?=.*[A-Z])";
                string pattern2 = "^[0-9a-zA-Z]{2,20}$";
                if (!Regex.IsMatch(userModel.Password, pattern1) ||
                    !Regex.IsMatch(userModel.Password, pattern2))
                    return new ValidationResult(Resources.Error_IncorrectUserPassword);

                return ValidationResult.Success;
            }
            
            throw new Exception(Resources.Error_IncorrectType);
        }
    }
}