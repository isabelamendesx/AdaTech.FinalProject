using Model.Domain.Entities;
using Model.Service.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Service.Services.Util
{
    internal static class RuleToFuncConverter
    {
        internal static IEnumerable<Func<decimal, RuleFuncResult>> ConvertListOfRules(
            IEnumerable<Rule?> rules)
        {
            List<Func<decimal, RuleFuncResult>> funcs = new List<Func<decimal, RuleFuncResult>>();

            foreach (var rule in rules)
            {
                Func<decimal, RuleFuncResult> ruleFunc;

                ruleFunc = (x) =>
                {
                    return new RuleFuncResult
                    {
                        FuncResult = x >= rule.MinValue && x <= rule.MaxValue,
                        Rule = rule
                    };
                };

                funcs.Add(ruleFunc);
            }

            return funcs;
        }
    }
}
