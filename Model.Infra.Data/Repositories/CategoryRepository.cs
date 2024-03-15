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
            try
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return category;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while trying to add category '{@CategoryName}' to the Database.", category.Name);
                throw;
            }
        }

        public async Task<Category?> GetById(uint Id, CancellationToken ct)
        {
            try
            {
                return await _context.Categories.FindAsync(Id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while trying to fetch Category with ID {@CategoryId} in the Database.", Id);
                throw;
            }
        } 

        public async Task<IEnumerable<Category?>> GetByParameter(CancellationToken ct, Expression<Func<Category, bool>> filter = null)
        {
            try
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
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while trying to fetch Categories with filter in the Database: {@Filter}", filter?.ToString() ?? "No filter applied");
                throw;
            }
        }

        public async Task UpdateAsync(Category category, CancellationToken ct)
        {
            try
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while updating Category with ID {@CategoryID} in the Database", category.Id);
                throw;
            }
        }
    }
}
