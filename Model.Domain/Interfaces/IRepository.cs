using Model.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T?>> GetByParameter(Expression<Func<T, bool>> filter);
        Task<T> AddAsync(T entity);
        Task<T?> GetById(int Id);
        Task UpdateAsync(T entity);
    }
}
