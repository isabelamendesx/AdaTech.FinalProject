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
        public int StatusCode { get; set; }

    }
}
