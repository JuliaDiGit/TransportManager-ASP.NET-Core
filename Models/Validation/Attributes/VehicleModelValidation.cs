using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Models.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class VehicleModelValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (value is VehicleModel vehicleModel)
            {
                //не больше 30 символов
                if (vehicleModel.Model.Length > 30) return new ValidationResult(Resources.Error_VehicleModelIsLonger);
                
                //буква-три цифры-две буквы- цифры региона.
                string numberPattern = "^[а-яА-Я]{1}[0-9]{3}[а-яА-Я]{2}[0-9]{2,3}$";
                if (!Regex.IsMatch(vehicleModel.GovernmentNumber, numberPattern))
                    return new ValidationResult(Resources.Error_IncorrectGovernmentNumber);
                
                return ValidationResult.Success;
            }
            
            throw new Exception(Resources.Error_IncorrectType);
        }
    }
}