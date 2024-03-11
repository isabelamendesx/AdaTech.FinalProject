using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Infra.Data.Context;
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
        private readonly ILogger _logger;

        public RuleRepository(ILogger logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Rule> AddAsync(Rule rule)
        {
            await _context.Rules.AddAsync(rule);
            await _context.SaveChangesAsync();
            return rule;
        }

        public async Task<IEnumerable<Rule?>> GetAll() => await _context.Rules.ToListAsync();

        public async Task<Rule?> GetById(uint Id) => await _context.Rules.FindAsync(Id);

        public async Task<IEnumerable<Rule?>> GetByParameter(Expression<Func<Rule, bool>> filter = null)
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

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to fetch Rules with filter: {FilterExpression}", filter?.ToString());
                throw;
            }
        }

        public async Task UpdateAsync(Rule rule)
        {
            try
            {
                _context.Rules.Update(rule);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while updating Rule. Details: {ErrorMessage}", ex.Message);
                throw new DbUpdateException("Error updating Rule. See inner exception for details.", ex);
            }
        }
    }
}
