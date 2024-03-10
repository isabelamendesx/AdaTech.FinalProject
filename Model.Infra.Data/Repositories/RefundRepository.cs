using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Infra.Data.Context;
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
        private readonly ILogger _logger;

        public RefundRepository(DataContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<Refund> AddAsync(Refund refund)
        {
            await _context.Refunds.AddAsync(refund);
            await _context.SaveChangesAsync();
            return refund;
        }

        public async Task UpdateAsync(Refund refund)
        {
            try
            {
                _context.Update(refund);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while updating the refund. Details: {ErrorMessage}", ex.Message);
                throw new DbUpdateException("Error updating refund. See inner exception for details.", ex);
            }
        }

        public async Task<IEnumerable<Refund?>> GetByParameter(Expression<Func<Refund, bool>> filter)
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

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to fetch Refunds with filter: {FilterExpression}", filter?.ToString() ?? "No filter applied");
                throw;
            }
        }

        public Task<Refund?> GetById(int Id) => _context.Refunds.FirstOrDefaultAsync(x => x.Id == Id);
    }
}
