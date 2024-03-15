using System.ComponentModel.DataAnnotations;

namespace Model.Application.API.DTO.Validators
{
    public class MinValueOrMaxValueValidatorAttibute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var minValueProperty = validationContext.ObjectType.GetProperty("MinValue");
            var maxValueProperty = validationContext.ObjectType.GetProperty("MaxValue");

            if (minValueProperty == null && maxValueProperty == null)
            {
                return new ValidationResult("You must specify at least one of the range limits and it must be a valid decimal.");
            }

            var minValue = minValueProperty == null ? 0 : (decimal)minValueProperty.GetValue(validationContext.ObjectInstance, null);
            var maxValue = maxValueProperty == null ? 0 : (decimal)maxValueProperty.GetValue(validationContext.ObjectInstance, null);

           

            if(minValue == 0 && maxValue == 0)
                return new ValidationResult("You must specify at least one of the range limits.");

            bool invalidRange = (minValue != 0 && maxValue != 0 && maxValue <= minValue) ||
                (minValue != 0 && maxValue == 0 && minValue == decimal.MaxValue);
                

            if(invalidRange)
                return new ValidationResult("You must specify a valid range.");

            return ValidationResult.Success;
        }
    }
}
