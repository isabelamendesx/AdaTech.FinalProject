using Microsoft.Extensions.Logging;
using Model.Domain.Common;
using Model.Domain.Interfaces;
using Model.Service.Exceptions;
using Model.Service.Services.Util;
using System.Data;
using Rule = Model.Domain.Entities.Rule;

namespace Model.Service.Services
{
    public class RuleService : IRuleService
    {
        private readonly IRepository<Rule> _ruleRepository;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<RuleService> _logger;

        public RuleService(IRepository<Rule> repository, ICategoryService categoryService, ILogger<RuleService> logger)
        {
            _ruleRepository = repository;
            _categoryService = categoryService;
            _logger = logger;
        }
        public async Task<Rule?> GetById(uint id, CancellationToken ct)
        {
            var rule = await _ruleRepository.GetById(id, ct);

            if (rule is null)
            {
                _logger.LogInformation("Rule with ID {@RuleId} not found", id);
                throw new ResourceNotFoundException("Rule");
            }

            return rule;
        }
        public async Task<IEnumerable<Rule?>> GetAll(CancellationToken ct)
        {
            return await _ruleRepository.GetByParameter(ct);
        }
        public async Task<Rule> CreateRule(Rule rule, CancellationToken ct)
        {
            List<Rule?> existingRules;

            var category = await _categoryService.GetById(rule.Category.Id, ct);

            if (rule.Category.Id == 0)
                existingRules = (List<Rule?>)await _ruleRepository.GetByParameter(ct, x => x.IsActive);
            else
                existingRules = (List<Rule?>)await _ruleRepository.GetByParameter(ct, 
                    x => (x.Category.Id == 0 || x.Category.Id == rule.Category.Id) && x.IsActive);

            RuleConflictAndOverlapChecker.CheckForConflictAndOverlap(rule, existingRules);

            return await _ruleRepository.AddAsync(rule, ct);
        }

        public async Task<bool> DeactivateACategorysRules(uint categoryId, CancellationToken ct)
        {
            IEnumerable<Rule?> rulesToDeactivate = await _ruleRepository.GetByParameter(ct, (x => x.Category.Id == categoryId && x.IsActive));

            if (rulesToDeactivate is null)
            {
                _logger.LogInformation("Attempt to deactived Rules for Category with ID {@RuleId} not found", categoryId);
                throw new ResourceNotFoundException("Rules");
            }

            foreach (Rule rule in rulesToDeactivate)
            {
                rule.IsActive = false;
                await _ruleRepository.UpdateAsync(rule, ct);
            }
            
            return true;
        }

        public async Task<bool> DeactivateRule(uint Id, CancellationToken ct)
        {
            var rule = await _ruleRepository.GetById(Id, ct);

            if (rule is null)
            {
                _logger.LogInformation("Attempt to deactivate Rule with ID {@RuleId} not found", Id);
                throw new ResourceNotFoundException("Rule");
            }

            rule.IsActive = false;

            await _ruleRepository.UpdateAsync(rule, ct);

            return true;
        }

        public async Task<IEnumerable<uint>> GetACategorysActiveRulesId(uint categoryId, CancellationToken ct)
        {
            var list = await _ruleRepository.GetByParameter(ct, x => x.Category.Id == categoryId && x.IsActive);

            IEnumerable<uint> ids = new List<uint>();

            for (var i = 0; i < list.Count(); i++)
                ids.Append(list.ElementAt(i)!.Id);

            return ids;
        }

        public async Task<IEnumerable<Rule>> GetRulesThatApplyToCategory(uint categoryId, CancellationToken ct)
        {
            var list = await _ruleRepository.GetByParameter(ct, 
                x => (x.Category.Id == 0  || x.Category.Id == categoryId) && x.IsActive);

            list = list.Where(rule => rule is not null);

            var rules = list
                .OrderBy(rule => rule.Action)
                .ThenBy(rule => rule.Category.Id);

            return rules;
        }

        public async Task<PaginatedResult<Rule>> GetAllPaginated(CancellationToken ct, int skip, int take)
        {
            return await _ruleRepository.GetPaginatedByParameter(ct, skip, take);
        }
    }
}
