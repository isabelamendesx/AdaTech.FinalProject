using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Infra.Data.Context;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Model.Infra.Data.Repositories
{
    public class RuleRepository : IRepository<Rule>
    {
        private readonly DataContext _context;

        public RuleRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Rule> AddAsync(Rule rule, CancellationToken ct)
        {
            try
            {
                await _context.Rules.AddAsync(rule);
                await _context.SaveChangesAsync();
                return rule;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while trying to add a new Rule to the Database.");
                throw;
            }

        }
        public async Task<Rule?> GetById(uint Id, CancellationToken ct)
        {
            try
            {
                return await _context.Rules.FindAsync(Id);
            } 
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while trying to fetch Rule with ID {@RefundOpId} in the Database.", Id);
                throw;
            }
        } 

        public async Task<IEnumerable<Rule?>> GetByParameter(CancellationToken ct, Expression<Func<Rule, bool>> filter = null)
        {
            try
            {
                var query = _context.Rules.AsQueryable();

                if (filter != null)
                {
                    query = query
                         .Where(filter)
                         .AsNoTrackingWithIdentityResolution();
                }

                return await query
                    .Include(x => x.Category)
                    .ToListAsync();
                   
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while trying to fetch Rules with filter in the Database: {@Filter}", filter?.ToString() ?? "No filter applied");
                throw;
            }
        }

        public async Task UpdateAsync(Rule rule, CancellationToken ct)
        {
            try
            {
                _context.Rules.Update(rule);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while updating Rule with ID {@RefundOpID} in the Database", rule.Id);
                throw;
            }
        }
    }
}
