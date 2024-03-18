using Model.Domain.Common;
using System.Linq.Expressions;

namespace Model.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T?>> GetByParameter(CancellationToken ct, Expression<Func<T, bool>> filter = null);
        Task<PaginatedResult<T>> GetPaginatedByParameter(CancellationToken ct, int skip, int take,
            Expression<Func<T, bool>> filter = null);
        Task<T> AddAsync(T entity, CancellationToken ct);
        Task<T?> GetById(uint Id, CancellationToken ct);
        Task UpdateAsync(T entity, CancellationToken ct);
    }
}
