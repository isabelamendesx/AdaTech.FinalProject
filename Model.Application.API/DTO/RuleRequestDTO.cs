using Model.Application.API.DTO.Validators;
using Model.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Model.Application.API.DTO
{
    public class RuleRequestDTO {
        [NotNull]
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
        [ValidateRuleAction]
        public string Action { get; set; }
        [NotNull]
        public uint CategoryId { get; set; }
    
    }
}
