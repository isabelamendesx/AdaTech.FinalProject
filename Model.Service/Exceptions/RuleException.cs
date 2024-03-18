namespace Model.Service.Exceptions
{
    public class RuleException : Exception
    {
        public uint ConflictingRuleId { get; }
        public int StatusCode { get; }
        public string Details { get; }

        public RuleException(string message, uint conflictingRuleId, int statusCode, string details)
            : base(message)
        {
            ConflictingRuleId = conflictingRuleId;
            StatusCode = statusCode;
            Details = details;
        }
    }
}
