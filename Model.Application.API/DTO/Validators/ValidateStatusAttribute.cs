using Model.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Model.Application.DTO.Validators
{
    public class ValidateStatusAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !string.Equals(value.ToString(), "Submitted", StringComparison.OrdinalIgnoreCase))
                return new ValidationResult("Only refunds with status 'Submitted' can be created.");

            return ValidationResult.Success;
        }
    }
}