using Model.Application.API.DTO.Response;
using Model.Domain.Entities;

namespace Model.Application.API.Extensions
{
    public static class RuleExtensions
    {
        public static RuleResponseDTO ToResponse(this Rule rule)
        {
            var action = rule.Action ? "Approve" : "Reject";

            return new RuleResponseDTO()
            {
                CategoryName = rule.Category.Name,
                MinValue = rule.MinValue,
                MaxValue = rule.MaxValue,
                Action = action,
                IsActived = rule.IsActive
            };
        }
    }
}
