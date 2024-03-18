using System.ComponentModel.DataAnnotations;

namespace Model.Application.API.DTO.Validators
{
    public class ValidateRuleActionAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !(new List<string>{ "Approve", "Reject" }).Any(v => v.Equals(value.ToString(), StringComparison.OrdinalIgnoreCase)))
                return new ValidationResult("Invalid action!");

            return ValidationResult.Success;
        }

        
    }
}
