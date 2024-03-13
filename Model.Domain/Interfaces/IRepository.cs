using Model.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.Interfaces
{
    public interface IRepository<T> 
    {
        Task<IEnumerable<T?>> GetByParameter(Expression<Func<T, bool>> filter = null, CancellationToken ct);
        Task<T> AddAsync(T entity, CancellationToken ct);
        Task<T?> GetById(uint Id, CancellationToken ct);
        Task UpdateAsync(T entity, CancellationToken ct);
    }
}
