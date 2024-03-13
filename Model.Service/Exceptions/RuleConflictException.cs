using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Service.Exceptions
{
    public class RuleConflictException : Exception
    {
        public uint ConflictingRuleId { get; }
        public int StatusCode { get; }
        public string Details { get; }

        public RuleConflictException(uint conflictingRuleId) 
            : base("Cannot create a new rule if it conflicts with an active rule")
        {
            ConflictingRuleId = conflictingRuleId;
            StatusCode = 404;
            Details = "Please deactivate the conflicting rule or modify the new rule being created";
        }
    }
}
