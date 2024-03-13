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
        public async Task<Rule?> GetById(uint id, CancellationToken ct)
        {
            return await _repository.GetById(id, ct);
        }
        public async Task<IEnumerable<Rule?>> GetAll(CancellationToken ct)
        {
            return await _repository.GetByParameter(ct);
        }
        public async Task<Rule> CreateRule(Rule rule, CancellationToken ct)
        {
            return await _repository.AddAsync(rule, ct);
        }

        public async Task<bool> DeactivateACategorysRules(uint categoryId, CancellationToken ct)
        {
            IEnumerable<Rule?> rulesToDeactivate = await _repository.GetByParameter(ct, (x => x.Category.Id == categoryId));

            if (rulesToDeactivate.Count() == 0)
                return false;

            foreach (Rule rule in rulesToDeactivate)
            {
                rule.IsActive = false;
                await _repository.UpdateAsync(rule, ct);
            }
            
            return true;
        }

        public async Task<bool> DeactivateRule(uint Id, CancellationToken ct)
        {
            var list = await _repository.GetByParameter(ct, (x => x.Id == Id));

            if (list.Count() == 0)
                return false;

            var rule = list.First();

            rule.IsActive = false;

            await _repository.UpdateAsync(rule, ct);

            return true;
        }

        public async Task<IEnumerable<Rule?>> GetRulesToApproveAny(CancellationToken ct)
        {
            var list = await _repository.GetByParameter(ct, x => x.Category.Id == 0 && x.Action == true && x.IsActive == true);
            return list;
        }

        public async Task<IEnumerable<Rule?>> GetRulesToApproveByCategoryId(uint categoryId, CancellationToken ct)
        {
            var list = await _repository.GetByParameter(ct, x => x.Category.Id == categoryId && x.IsActive == true);
            return list;
        }

        public async Task<IEnumerable<Rule?>> GetRulesToReproveAny(CancellationToken ct)
        {
            var list = await _repository.GetByParameter(ct, x => x.Category.Id == 0 && x.Action == false && x.IsActive == true);
            return list;
        }
    }
}
