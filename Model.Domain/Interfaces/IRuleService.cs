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
        Task<Rule> CreateRule(Rule rule, CancellationToken ct);
        Task<IEnumerable<Rule?>> GetRulesToRejectAny(CancellationToken ct);
        Task<IEnumerable<Rule?>> GetRulesToApproveAny(CancellationToken ct);
        Task<IEnumerable<Rule?>> GetRulesToRejectByCategoryId(uint categoryId, CancellationToken ct);
        Task<IEnumerable<Rule?>> GetRulesToApproveByCategoryId(uint categoryId, CancellationToken ct);
        Task<bool> DeactivateRule(uint Id, CancellationToken ct);
        Task<bool> DeactivateACategorysRules(uint categoryId, CancellationToken ct);
    }
}
