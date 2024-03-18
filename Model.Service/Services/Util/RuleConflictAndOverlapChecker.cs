using Model.Domain.Entities;
using Model.Service.Exceptions;
using Serilog;

namespace Model.Service.Services.Util
{
    public static class RuleConflictAndOverlapChecker
    {
        public static void CheckForConflictAndOverlap(Rule newRule, List<Rule?> existingRules)
        {
            foreach(var rule in existingRules)
            {
                if(rule is not null)
                {
                    if (HasConflictingIntervals(rule, newRule))
                    {
                        if (rule.Action != newRule.Action)
                        {
                            Log.Warning("Attempted to create rule for Category'{@CategoryName}' which conflicts with an existing rule with id {@RuleID}", newRule.Category.Name, rule.Id);
                            throw new RuleConflictException(rule.Id);
                        }
                        else
                        {
                            Log.Warning("Attempted to create rule for Category '{CategoryName}' which overlaps with an existing rule with id {@RuleID}", newRule.Category.Name, rule.Id);
                            throw new RuleOverlapException(rule.Id);
                        }
                    }
                }
            }
        }

        private static bool HasConflictingIntervals(Rule rule, Rule newRule)
        {
            return rule.MinValue <= newRule.MaxValue && newRule.MinValue <= rule.MaxValue;

        }

    }
}
