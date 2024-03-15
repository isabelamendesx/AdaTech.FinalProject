using System.ComponentModel.DataAnnotations;

namespace Model.Application.API.DTO.Validators
{
    public class MinValueOrMaxValueRequiredAttibute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var minValueProperty = validationContext.ObjectType.GetProperty("MinValue");
            var maxValueProperty = validationContext.ObjectType.GetProperty("MaxValue");

            var minValue = (decimal)minValueProperty.GetValue(validationContext.ObjectInstance, null);
            var maxValue = (decimal)maxValueProperty.GetValue(validationContext.ObjectInstance, null);

            if(minValue == 0 && maxValue == 0)
                return new ValidationResult("You must specify at least one of the range limits.");

            return ValidationResult.Success;
        }
    }
}
