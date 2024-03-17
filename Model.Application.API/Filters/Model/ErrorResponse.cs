using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Model.Application.API.Filters.Model
{
    public class ErrorResponse
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string? Details { get; set; }
        public DateTime TimeStamp { get; set; }
        public string RequestPath { get; set; }
        public uint? ConflictingRuleId { get; set; }
        public uint? ConflictingCategoryId { get; set; }
        public int StatusCode { get; set; }
        public IDictionary<string, string[]> Errors { get; set; }
    }
}
