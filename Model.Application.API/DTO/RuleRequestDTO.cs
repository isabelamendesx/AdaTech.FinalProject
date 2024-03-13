using Model.Application.API.DTO.Validators;
using Model.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Model.Application.API.DTO
{
    public class RuleRequestDTO {

        [NotNull]
        [Range(0, double.MaxValue, ErrorMessage = "MinValue must be a positive number")]
        public decimal MinValue { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "MaxValue must be a positive number")]
        public decimal MaxValue { get; set; }

        [ValidateRuleAction]
        [Required(ErrorMessage = "Action is required")]
        public string Action { get; set; }

        [NotNull]
        [Required(ErrorMessage = "CategoryId is required")]
        [Range(1, uint.MaxValue, ErrorMessage = "CategoryId must be a positive number")]
        public uint CategoryId { get; set; }
    
    }
}
