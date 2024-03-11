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
    public class CategoryRepository : IRepository<Category>
    {
        private readonly DataContext _context;
        private readonly ILogger _logger;

        public CategoryRepository(ILogger logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Category> AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> GetById(uint Id) => await _context.Categories.FindAsync(Id);

        public async Task<IEnumerable<Category?>> GetByParameter(Expression<Func<Category, bool>> filter = null)
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
                _logger.LogError(ex, "An error occurred while trying to fetch Rules with filter: {FilterExpression}", filter?.ToString());
                throw;
            }
        }

        public async Task UpdateAsync(Category category)
        {
            try
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while updating Category. Details: {ErrorMessage}", ex.Message);
                throw new DbUpdateException("Error updating Category. See inner exception for details.", ex);
            }
        }
    }
}
