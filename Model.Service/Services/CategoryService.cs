using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Service.Exceptions;
using System;
using System.Collections.Generic;
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

        public async Task<Category> CreateCategory(Category category)
        {
            var sameNameCategories = _repository
                .GetByParameter(
                    x => x.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase)).Result;

            if (sameNameCategories.Count() != 0)
                throw new CategoryAlreadyRegisteredException();

            return await _repository.AddAsync(category);
        }

        public async Task<IEnumerable<Category?>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<Category?> GetById(uint id)
        {
            return await _repository.GetById(id);
        }
    }
}
