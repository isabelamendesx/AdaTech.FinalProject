using Model.Domain.Entities;
using Model.Service.Services.DTO;
using Model.Service.Services.Util;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Service.Services.Handlers
{
    internal class ApprovalMotorHandler
    {
        private Rule _rule;
        private ApprovalMotorHandler _nextHandler;

        public ApprovalMotorHandler(Rule rule, ApprovalMotorHandler nextHandler)
        {
            _rule = rule;
            _nextHandler = nextHandler;
        }

        public ProcessRefundResult Handle(decimal value)
        {
            EStatus possibleStatus = _rule.Action ? EStatus.Approved : EStatus.Rejected;
            Log.Information($"Iniciando com refund de categorya {_rule.Category.Id} e action {_rule.Action}");

            var func = RuleToFuncConverter.ConvertRuleToFunc(_rule);

            var result = func(value);

            if(result.FuncResult)
                return new ProcessRefundResult() { Status = possibleStatus, Rule = result.Rule };

            if(_nextHandler != null) return _nextHandler.Handle(value);

            return new ProcessRefundResult() { Status = EStatus.UnderEvaluation, Rule = null }; ;
        }

        public static ApprovalMotorHandler CreateChain(IEnumerable<Rule> rules)
        {
            ApprovalMotorHandler handler = null;

            for (int i = rules.Count() - 1; i >= 0; i--)
            {
                handler = new ApprovalMotorHandler(rules.ElementAt(i), handler);
            }

            return handler;
        }
    }
}
