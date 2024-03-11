using Model.Domain.Entities;
using Model.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Service.Services
{
    public class RuleService : IRuleService
    {
        private readonly IRepository<Rule> _repository;

        public RuleService(IRepository<Rule> repository)
        {
            _repository = repository;
        }

        public Task<Rule> CreateRule(Rule rule)
        {
            return _repository.AddAsync(rule);
        }

        public async Task<bool> DeactivateACategorysRules(int categoryId)
        {
            IEnumerable<Rule?> rulesToDeactivate = await _repository.GetByParameter(x => x.Category.Id == categoryId);

            if (rulesToDeactivate.Count() == 0)
                return false;

            foreach (Rule rule in rulesToDeactivate)
            {
                rule.IsActive = false;
                await _repository.UpdateAsync(rule);
            }
            
            return true;
        }

        public async Task<bool> DeactivateRule(int Id)
        {
            var list = await _repository.GetByParameter(x => x.Id == Id);

            if (list.Count() == 0)
                return false;

            var rule = list.First();

            rule.IsActive = false;

            await _repository.UpdateAsync(rule);

            return true;
        }

        public async Task<IEnumerable<Rule?>> GetRulesToApproveAny()
        {
            var list = await _repository.GetByParameter(x => x.Category.Id == 0 && x.Action == true);
            return list;
        }

        public async Task<IEnumerable<Rule?>> GetRulesToApproveByCategoryId(int categoryId)
        {
            var list = await _repository.GetByParameter(x => x.Category.Id == categoryId);
            return list;
        }

        public async Task<IEnumerable<Rule?>> GetRulesToReproveAny()
        {
            var list = await _repository.GetByParameter(x => x.Category.Id == 0 && x.Action == false);
            return list;
        }
    }
}
