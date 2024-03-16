using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.Domain.Common;
using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Infra.Data.Context;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Model.Infra.Data.Repositories
{
    public class RefundRepository : IRepository<Refund>
    {
        private readonly DataContext _context;

        public RefundRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Refund> AddAsync(Refund refund, CancellationToken ct)
        {
            await _context.Refunds.AddAsync(refund);
            await _context.SaveChangesAsync();
            return refund;
        }

        public async Task UpdateAsync(Refund refund, CancellationToken ct)
        {
            _context.Refunds.Update(refund);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Refund?>> GetByParameter(CancellationToken ct, Expression<Func<Refund, bool>> filter = null)
        {
                var query = _context.Refunds.AsQueryable();

                if (filter != null)
                {
                    query = query
                         .Where(filter)
                         .AsNoTrackingWithIdentityResolution();
                }

                return await query.Include(x => x.Category)
                    .Include(x => x.Operations)
                    .ThenInclude(x => x.ApprovalRule)
                    .ThenInclude(x => x.Category)
                    .ToListAsync();
        }

        public async Task<Refund?> GetById(uint Id, CancellationToken ct)
               => await _context.Refunds.FirstOrDefaultAsync(x => x.Id == Id);

        public async Task<PagedResult<Refund>> GetPagedByParameter(CancellationToken ct, int skip, int take, Expression<Func<Refund, bool>> filter = null)
        {
            var query = _context.Refunds.AsQueryable();

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

            return new PagedResult<Refund> { TotalCount = totalCount, Items = items };
        }
    }
}
