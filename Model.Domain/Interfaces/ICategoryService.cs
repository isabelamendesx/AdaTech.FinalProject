using Model.Domain.Common;
using Model.Domain.Entities;

namespace Model.Domain.Interfaces
{
    public interface ICategoryService
    {
        Task<Category> GetById(uint id, CancellationToken ct);
        Task<IEnumerable<Category?>> GetAll(CancellationToken ct);
        Task<PaginatedResult<Category>> GetAllPaginated(CancellationToken ct, int skip, int take);
        Task<Category> CreateCategory(Category category, CancellationToken ct);
    }
}
