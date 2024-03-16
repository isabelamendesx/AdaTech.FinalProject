using Microsoft.EntityFrameworkCore;
using Model.Domain.Common;
using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Infra.Data.Context;
using Serilog;
using System.Data;
using System.Linq;
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
            await _context.RefundOperations.AddAsync(operation);
            await _context.SaveChangesAsync();
            return operation;
        }

        public async Task<RefundOperation?> GetById(uint Id, CancellationToken ct)
            => await _context.RefundOperations.FindAsync(Id);

        public async Task<IEnumerable<RefundOperation?>> GetByParameter(CancellationToken ct, Expression<Func<RefundOperation, bool>> filter = null)
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

        public async Task<PaginatedResult<RefundOperation>> GetPaginatedByParameter(CancellationToken ct, int skip, int take, Expression<Func<RefundOperation, bool>> filter = null)
        {
            var query = _context.RefundOperations.AsQueryable();

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

            return new PaginatedResult<RefundOperation> { TotalCount = totalCount, Items = items };
        }

        public async Task UpdateAsync(RefundOperation operation, CancellationToken ct)
        {
            _context.RefundOperations.Update(operation);
             await _context.SaveChangesAsync();
        }
    }
}
