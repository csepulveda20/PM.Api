using PM.Application.Repository;
using PM.Domain.Entities;
using PM.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace PM.Infrastructure.Services
{
    public class CategoryService : ICategory
    {
        private readonly DatabaseContext _context;
        private readonly UnitOfWork _unitOfWork;
        public CategoryService(DatabaseContext context, UnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Category>> ListAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task CreateAsync(Category category)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateAsync(int id, string name, int parentCategoryId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null) throw new KeyNotFoundException("Categoría no encontrada");
                category.Name = name;
                category.ParentCategoryId = parentCategoryId;
                await _context.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task PatchIsActiveAsync(int id, bool isActive)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null) throw new KeyNotFoundException("Categoría no encontrada");
                category.IsActive = isActive;
                await _context.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
