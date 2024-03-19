using System.ComponentModel.DataAnnotations;

namespace Model.Application.API.DTO.Validators
{
    public class ValidateChangeStatusAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            List<string> possibleStatus = new List<string>()
            {
                "underevaluation",
                "approved",
                "rejected"
            };
            if (value == null || !possibleStatus.Contains(value.ToString().ToLower()))
                return new ValidationResult("You can only change the status to 'UnderEvaluation', 'Approved' or 'Rejected'.");

            return ValidationResult.Success;
        }
    }
}
