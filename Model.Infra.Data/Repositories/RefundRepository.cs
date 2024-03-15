using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
            try
            {
                await _context.Refunds.AddAsync(refund);
                await _context.SaveChangesAsync();
                return refund;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while trying to add a new Refund to the Database.");
                throw;
            }

        }

        public async Task UpdateAsync(Refund refund, CancellationToken ct)
        {
            try
            {
                _context.Refunds.Update(refund);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, "An error occurred while updating Refund with ID {@RefundID} in the Database", refund.Id);
                throw;
            }
        }

        public async Task<IEnumerable<Refund?>> GetByParameter(CancellationToken ct, Expression<Func<Refund, bool>> filter = null)
        {
            try
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
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while trying to fetch Refunds with filter in the Database: {@Filter}", filter?.ToString() ?? "No filter applied");
                throw;
            }
        }

        public async Task<Refund?> GetById(uint Id, CancellationToken ct)
        {
            try
            {
                return await _context.Refunds.FirstOrDefaultAsync(x => x.Id == Id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while trying to fetch Refund with ID {@RefundId} in the Database.", Id);
                throw;
            }
        }

    }
}
