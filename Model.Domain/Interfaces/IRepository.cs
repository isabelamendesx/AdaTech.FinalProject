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
        Task<IEnumerable<T?>> GetByParameter(Expression<Func<T, bool>> filter = null);
        Task<T> AddAsync(T entity);
        Task<T?> GetById(int Id);
        Task UpdateAsync(T entity);
    }
}
