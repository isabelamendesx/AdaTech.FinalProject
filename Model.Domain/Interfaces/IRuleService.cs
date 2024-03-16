using Model.Domain.Common;
using Model.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.Interfaces
{
    public interface IRuleService
    {
        Task<Rule?> GetById(uint id, CancellationToken ct);
        Task<IEnumerable<Rule?>> GetAll(CancellationToken ct);
        Task<PagedResult<Rule>> GetAllPaged(CancellationToken ct, int skip, int take);
        Task<Rule> CreateRule(Rule rule, CancellationToken ct);
        Task<bool> DeactivateRule(uint Id, CancellationToken ct);
        Task<bool> DeactivateACategorysRules(uint categoryId, CancellationToken ct);
        Task<IEnumerable<uint>> GetACategorysActiveRulesId(uint categoryId, CancellationToken ct);
        Task<IEnumerable<Rule>> GetRulesThatApplyToCategory(uint categoryId, CancellationToken ct);
    }
}
