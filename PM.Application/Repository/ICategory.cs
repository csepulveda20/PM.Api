using PM.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PM.Application.Repository
{
    public interface ICategory
    {
        Task<IEnumerable<Category>> ListAsync();
        Task<Category> GetByIdAsync(int id);
        Task CreateAsync(Category category);
        Task UpdateAsync(int id, string name, int parentCategoryId);
        Task PatchIsActiveAsync(int id, bool isActive);
    }
}
