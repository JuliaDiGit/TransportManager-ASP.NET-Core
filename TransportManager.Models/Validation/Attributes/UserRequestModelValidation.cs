using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using TransportManager.Common.Enums;

namespace TransportManager.Models.Validation.Attributes
{
    public class UserRequestModelValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (value is UserRequestModel userRequestModel)
            {
                // до 15 знаков, не должно быть цифр и спецсимволов.
                string loginPattern = "^[a-zA-Z]{1,15}$";
                if (!Regex.IsMatch(userRequestModel.Login, loginPattern))
                    return new ValidationResult(Resources.Error_IncorrectUserLogin);

                // до 20 символов, обязательна хотя бы 1 заглавная и 1 цифра, не должно быть спецсимволов.
                string pattern1 = "(?=.*[0-9])(?=.*[A-Z])";
                string pattern2 = "^[0-9a-zA-Z]{2,20}$";
                if (!Regex.IsMatch(userRequestModel.Password, pattern1) 
                    || !Regex.IsMatch(userRequestModel.Password, pattern2)) 
                    return new ValidationResult(Resources.Error_IncorrectUserPassword);

                // значение Роли должно быть в пределах количества элементов перечисления
                if (userRequestModel.Role < 0 
                    || (int)userRequestModel.Role >= Enum.GetNames(typeof(Role)).Length) 
                    return new ValidationResult(Resources.Error_IncorrectRole);
                

                return ValidationResult.Success;
            }
            
            throw new Exception(Resources.Error_IncorrectType);
        }
    }
}