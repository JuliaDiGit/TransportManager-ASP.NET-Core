using System;
using System.ComponentModel.DataAnnotations;

namespace Models.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CompanyModelValidation : ValidationAttribute
    { 
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (value is CompanyModel companyModel)
            {
                //не больше 50 символов
                if (companyModel.CompanyName.Length > 50) return new ValidationResult(Resources.Error_CompanyNameIsLonger);
                //в диапазоне от 1 до 999999
                if (companyModel.CompanyId < 1 || companyModel.CompanyId > 999999)
                    return new ValidationResult(Resources.Error_CompanyIdOutOfRange);

                return ValidationResult.Success;
            }

            throw new Exception(Resources.Error_IncorrectType);
        }
    }
}