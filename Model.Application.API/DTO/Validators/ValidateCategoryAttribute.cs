using Model.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Application.DTO.Validators
{
    public class ValidateCategoryAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !Enum.GetNames(typeof(ECategory)).Any(v => v.Equals(value.ToString(), StringComparison.OrdinalIgnoreCase)))
                return new ValidationResult("Invalid Category!");

            return ValidationResult.Success;
        }
    }
}
