using PM.Application.DTOs;
using PM.Application.Repository;
using Entity = PM.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PM.Application.UseCases.Category
{
    public class CategoryUseCase
    {
        private readonly ICategory _repo;
        public CategoryUseCase(ICategory repo)
        {
            _repo = repo;
        }

        public async Task<PaginateResponse<Entity.Category>> ListAsync(int page = 1, int pageSize = 10, string search = null, bool? isActive = null)
        {
            var all = await _repo.ListAsync();
            var filtered = all.AsQueryable();
            if (!string.IsNullOrEmpty(search))
                filtered = filtered.Where(c => c.Name.Contains(search));
            if (isActive.HasValue)
                filtered = filtered.Where(c => c.IsActive == isActive);
            int total = filtered.Count();
            var items = filtered.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return new PaginateResponse<Entity.Category>(items, page, pageSize, total);
        }

        public async Task<Entity.Category> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task CreateAsync(Entity.Category category)
        {
            await _repo.CreateAsync(category);
        }

        public async Task UpdateAsync(int id, string name, int parentCategoryId)
        {
            await _repo.UpdateAsync(id, name, parentCategoryId);
        }

        public async Task PatchIsActiveAsync(int id, bool isActive)
        {
            await _repo.PatchIsActiveAsync(id, isActive);
        }
    }
}
