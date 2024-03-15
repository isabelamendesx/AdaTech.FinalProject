using Microsoft.EntityFrameworkCore;
using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Infra.Data.Context;
using Serilog;
using System.Data;
using System.Linq.Expressions;

namespace Model.Infra.Data.Repositories
{
    public class RefundOperationRepository : IRepository<RefundOperation>
    {
        private readonly DataContext _context;

        public RefundOperationRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<RefundOperation> AddAsync(RefundOperation operation, CancellationToken ct)
        {
            try
            {
                await _context.RefundOperations.AddAsync(operation);
                await _context.SaveChangesAsync();
                return operation;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while trying to add a new Refund Operation to the Database.");
                throw;
            }

        }

        public async Task<RefundOperation?> GetById(uint Id, CancellationToken ct)
        {
            try
            {
                return await _context.RefundOperations.FindAsync(Id);
            } 
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while trying to fetch Refund Operation with ID {@RefundOpId} in the Database.", Id);
                throw;
            }
            
        }

        public async Task<IEnumerable<RefundOperation?>> GetByParameter(CancellationToken ct, Expression<Func<RefundOperation, bool>> filter = null)
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
                Log.Error(ex, "An error occurred while trying to fetch Refund Operations with filter in the Database: {@Filter}", filter?.ToString() ?? "No filter applied");
                throw;
            }
        }

        public async Task UpdateAsync(RefundOperation operation, CancellationToken ct)
        {
            try
            {
                _context.RefundOperations.Update(operation);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while updating Refund Operation with ID {@RefundOpID} in the Database", operation.Id);
                throw;
            }
        }
    }
}
