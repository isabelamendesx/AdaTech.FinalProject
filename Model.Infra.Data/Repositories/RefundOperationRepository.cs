using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Model.Infra.Data.Repositories
{
    public class RefundOperationRepository : IRepository<RefundOperation>
    {
        private readonly DataContext _context;
        private readonly ILogger _logger;

        public RefundOperationRepository(ILogger logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<RefundOperation> AddAsync(RefundOperation operation)
        {
            await _context.RefundOperations.AddAsync(operation);
            await _context.SaveChangesAsync();
            return operation;
        }

        public async Task<RefundOperation?> GetById(uint Id) => await _context.RefundOperations.FindAsync(Id);

        public async Task<IEnumerable<RefundOperation?>> GetByParameter(Expression<Func<RefundOperation, bool>> filter = null)
        {
            try
            {
                var query = _context.RefundOperations.AsQueryable();

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
                _logger.LogError(ex, "An error occurred while trying to fetch Operations with filter: {FilterExpression}", filter?.ToString());
                throw;
            }
        }

        public async Task UpdateAsync(RefundOperation operation)
        {
            try
            {
                _context.RefundOperations.Update(operation);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while updating the Operation. Details: {ErrorMessage}", ex.Message);
                throw new DbUpdateException("Error updating Operation. See inner exception for details.", ex);
            }
        }
    }
}
