using System;
using System.ComponentModel.DataAnnotations;

namespace TransportManager.Models.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DriverModelValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (value is DriverModel driverModel)
            {
                //не больше 50 символов
                if (driverModel.Name.Length > 50) return new ValidationResult(Resources.Error_DriverNameIsLonger);
                
                return ValidationResult.Success;
            }

            throw new Exception(Resources.Error_IncorrectType);
        }
    }
}