using Microsoft.Extensions.Logging;
using Model.Domain.Common;
using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Service.Exceptions;

namespace Model.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private IRepository<Category> _repository;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(IRepository<Category> repository, ILogger<CategoryService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Category> CreateCategory(Category category, CancellationToken ct)
        {
            var sameNameCategories = await _repository
                .GetByParameter(ct,
                    x => x.Name.ToLower().Equals(category.Name.ToLower()));

            if (sameNameCategories.Any())
            {
                _logger.LogWarning("Attempted to create category '{@CategoryName}' which already exists.", category.Name);
                throw new CategoryAlreadyRegisteredException(sameNameCategories.First().Id);
            }

            return await _repository.AddAsync(category, ct);
        }

        public async Task<IEnumerable<Category?>> GetAll(CancellationToken ct)
        {
            //_logger.LogInformation("Fetching all categories");
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
                _logger.LogInformation("Category with ID {@CategoryId} not found", id);
                throw new ResourceNotFoundException("Category");
            }

            //_logger.LogInformation("Category fetched: {@Category}", category);
            return category;
        }
    }
}
