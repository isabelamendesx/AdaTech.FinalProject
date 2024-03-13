using Model.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain.Interfaces
{
    public interface ICategoryService
    {
        Task<Category?> GetById(uint id, CancellationToken ct);
        Task<IEnumerable<Category?>> GetAll(CancellationToken ct);
        Task<Category> CreateCategory(Category category, CancellationToken ct);
    }
}
