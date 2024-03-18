namespace Model.Service.Exceptions
{
    public class RuleOverlapException : RuleException
    {
        public RuleOverlapException(uint conflictingRuleId)
            : base("Cannot create a new rule if it overlaps an active rule", conflictingRuleId, 404, "Please deactivate the conflicting rule or modify the new rule being created")
        {
        }
    }
}
