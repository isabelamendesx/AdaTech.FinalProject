using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.Interfaces
{
    public interface IFullRepository<T> : IPartialRepository<T>  
    {
        Task<T> DeleteAsync(T entity);
    }
}
