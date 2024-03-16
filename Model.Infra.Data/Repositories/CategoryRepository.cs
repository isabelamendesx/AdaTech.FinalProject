using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Infra.Data.Context;
using Serilog;
using System.Data;
using System.Linq.Expressions;

namespace Model.Infra.Data.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Category> AddAsync(Category category, CancellationToken ct)
        {
             await _context.Categories.AddAsync(category);
             await _context.SaveChangesAsync();
             return category;
        }

        public async Task<Category?> GetById(uint Id, CancellationToken ct)
                => await _context.Categories.FindAsync(Id);

        public async Task<IEnumerable<Category?>> GetByParameter(CancellationToken ct, Expression<Func<Category, bool>> filter = null)
        {
                var query = _context.Categories.AsQueryable();

                if (filter != null)
                {
                    query = query
                         .Where(filter)
                         .AsNoTrackingWithIdentityResolution();
                }

                return await query.ToListAsync();
        }

        public async Task UpdateAsync(Category category, CancellationToken ct)
        {
             _context.Categories.Update(category);
              await _context.SaveChangesAsync();
        }
    }
}
