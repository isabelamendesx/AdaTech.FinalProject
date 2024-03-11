using Model.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Service.Services
{
    public interface IRuleService
    {
        Task<Rule> CreateRule(Rule rule);
        Task<IEnumerable<Rule?>> GetRulesToReproveAny();
        Task<IEnumerable<Rule?>> GetRulesToApproveAny();
        Task<IEnumerable<Rule?>> GetRulesToApproveByCategoryId(int categoryId);
        Task<bool> DeactivateRule(int Id);
        Task<bool> DeactivateACategorysRules(int categoryId);
    }
}
