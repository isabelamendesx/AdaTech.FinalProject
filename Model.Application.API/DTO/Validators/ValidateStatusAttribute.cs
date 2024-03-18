using Model.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Model.Application.API.DTO.Validators
{
    public class ValidateStatusAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !Enum.GetNames(typeof(EStatus)).Any(v => v.Equals(value.ToString(), StringComparison.OrdinalIgnoreCase)))
                return new ValidationResult("Invalid Status!");

            return ValidationResult.Success;
        }
    }
}
