using System.ComponentModel.DataAnnotations;

namespace Model.Application.API.DTO.Validators
{
    public class ValidateChangeStatusAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            List<string> possibleStatus = new List<string>()
            {
                "submitted",
                "approved",
                "rejected"
            };
            if (value == null || !possibleStatus.Contains(value.ToString().ToLower()))
                return new ValidationResult("Only refunds with status 'Submitted' can be created.");

            return ValidationResult.Success;
        }
    }
}
