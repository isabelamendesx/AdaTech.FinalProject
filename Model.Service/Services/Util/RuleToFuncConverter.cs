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
        internal static Func<decimal, RuleFuncResult> ConvertRuleToFunc(
            Rule rule)
        {
            Func<decimal, RuleFuncResult> func;

            func = (x) =>
            {
                return new RuleFuncResult
                {
                    FuncResult = x >= rule.MinValue && x <= rule.MaxValue,
                    Rule = rule
                };
            };

            return func;
        }
    }
}
