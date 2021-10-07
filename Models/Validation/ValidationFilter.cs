using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Validation
{
    public static class ValidationFilter
    {
        public static void Validate(object value)
        {
            var validationContext = new ValidationContext(value);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(value, validationContext, validationResults))
            {
                validationResults.ForEach(x =>
                    throw new ValidationException(x.ErrorMessage));
            }
        }
    }
}