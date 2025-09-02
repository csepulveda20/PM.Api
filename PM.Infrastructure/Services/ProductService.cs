using PM.Application.Repository;
using PM.Domain.Entities;
using PM.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PM.Application.Common;

namespace PM.Infrastructure.Services
{
    public class ProductService : IProduct
    {
        private readonly DatabaseContext _context;
        private readonly UnitOfWork _unitOfWork;
        public ProductService(DatabaseContext context, UnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Product>> ListAsync(QueryParam queryParam, int? categoryId = null, bool? isActive = null)
        {
            var query = _context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(queryParam.Search))
                query = query.Where(p => p.Name.Contains(queryParam.Search) || p.Sku.Contains(queryParam.Search));
            if (categoryId != null)
                query = query.Where(p => p.CategoryId == categoryId);
            if (isActive.HasValue)
                query = query.Where(p => p.IsActive == isActive);
            if (!string.IsNullOrEmpty(queryParam.OrderBy))
            {
                // Simple order by name or price, extend as needed
                if (queryParam.OrderBy.ToLower() == "name")
                    query = query.OrderBy(p => p.Name);
                else if (queryParam.OrderBy.ToLower() == "price")
                    query = query.OrderBy(p => p.Price);
            }
            // Paginación
            int skip = (queryParam.PageNumber - 1) * queryParam.PageSize;
            return await query.Skip(skip).Take(queryParam.PageSize).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task CreateAsync(Product product)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateAsync(string id, string sku, string name, string description, decimal price, int categoryId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null) throw new KeyNotFoundException("Producto no encontrado");
                product.Sku = sku;
                product.Name = name;
                product.Description = description;
                product.Price = price;
                product.CategoryId = categoryId;
                await _context.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task PatchIsActiveAsync(string id, bool isActive)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null) throw new KeyNotFoundException("Producto no encontrado");
                product.IsActive = isActive;
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
