using Microsoft.EntityFrameworkCore;
using Model.Domain.Common;
using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Infra.Data.Context;
using System.Linq.Expressions;

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
              await _context.Rules.AddAsync(rule);
              await _context.SaveChangesAsync();
              return rule;
        }
        public async Task<Rule?> GetById(uint Id, CancellationToken ct)
                    => await _context.Rules
                                    .Include(x => x.Category)
                                    .FirstOrDefaultAsync(x => x.Id == Id);

        public async Task<IEnumerable<Rule?>> GetByParameter(CancellationToken ct, Expression<Func<Rule, bool>> filter = null)
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

        public async Task<PaginatedResult<Rule>> GetPaginatedByParameter(CancellationToken ct, int skip, int take, Expression<Func<Rule, bool>> filter = null)
        {
            var query = _context.Rules.AsQueryable();

            int totalCount = 0;

            if (filter != null)
            {
                query = query
                     .Where(filter)
                     .AsNoTrackingWithIdentityResolution();

            }

            var items = await query
                        .Skip(skip)
                        .Take(take)
                        .ToListAsync(ct);

            totalCount = await query.CountAsync(ct);

            return new PaginatedResult<Rule> { TotalCount = totalCount, Items = items };
        }

        public async Task UpdateAsync(Rule rule, CancellationToken ct)
        {
            _context.Rules.Update(rule);
            await _context.SaveChangesAsync();
        }

    }
}
