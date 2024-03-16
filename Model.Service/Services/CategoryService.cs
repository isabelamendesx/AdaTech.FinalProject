using Microsoft.Extensions.Logging;
using Model.Domain.Common;
using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Service.Exceptions;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private IRepository<Category> _repository;

        public CategoryService(IRepository<Category> repository)
        {
            _repository = repository;
        }

        public async Task<Category> CreateCategory(Category category, CancellationToken ct)
        {
            var sameNameCategories = await _repository
                .GetByParameter(ct,
                    x => x.Name.ToLower().Equals(category.Name.ToLower()));

            if (sameNameCategories.Any())
            {
                Log.Warning("Attempted to create category '{@CategoryName}' which already exists.", category.Name);
                throw new CategoryAlreadyRegisteredException(sameNameCategories.First().Id);
            }

            return await _repository.AddAsync(category, ct);
        }

        public async Task<IEnumerable<Category?>> GetAll(CancellationToken ct)
        {
            //Log.Information("Fetching all categories");
            return await _repository.GetByParameter(ct);
        }

        public async Task<PaginatedResult<Category>> GetAllPaginated(CancellationToken ct, int skip, int take)
        {
            return await _repository.GetPaginatedByParameter(ct, skip, take);
        }

        public async Task<Category?> GetById(uint id, CancellationToken ct)
        {
            var category = await _repository.GetById(id, ct);

            if (category is null)
            {
                Log.Information("Category with ID {@CategoryId} not found", id);
                throw new ResourceNotFoundException("Category");
            }

            //Log.Information("Category fetched: {@Category}", category);
            return category;
        }
    }
}
