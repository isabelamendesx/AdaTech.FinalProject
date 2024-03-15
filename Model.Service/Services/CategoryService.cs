using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Service.Exceptions;
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
                                     .GetByParameter(ct,x => x.Name.ToLower().Equals(category.Name.ToLower()));

            if (sameNameCategories.Any())
                throw new CategoryAlreadyRegisteredException();

            return await _repository.AddAsync(category, ct);
        }

        public async Task<IEnumerable<Category?>> GetAll(CancellationToken ct)
        {
            return await _repository.GetByParameter(ct);
        }

        public async Task<Category?> GetById(uint id, CancellationToken ct)
        {
            var category = await _repository.GetById(id, ct);
            if (category is null)
                throw new ResourceNotFoundException("Category");
            return category;
        }
    }
}
