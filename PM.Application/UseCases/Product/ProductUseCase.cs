using PM.Application.Common;
using PM.Application.DTOs;
using PM.Application.Repository;
using Entity = PM.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PM.Application.UseCases.Product
{
    public class ProductUseCase
    {
        private readonly IProduct _repo;
        public ProductUseCase(IProduct repo)
        {
            _repo = repo;
        }

        public async Task<PaginateResponse<Entity.Product>> ListAsync(QueryParam queryParam, int? categoryId = null, bool? isActive = null)
        {
            var items = await _repo.ListAsync(queryParam, categoryId, isActive);
            // Para el total, ejecuta la consulta sin paginación
            // (Idealmente, el repo debería retornar el total, pero aquí lo calculamos)
            int total = items is ICollection<Entity.Product> col ? col.Count : 0;
            return new PaginateResponse<Entity.Product>(items, queryParam.PageNumber, queryParam.PageSize, total);
        }

        public async Task<Entity.Product> GetByIdAsync(string id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task CreateAsync(Entity.Product product)
        {
            await _repo.CreateAsync(product);
        }

        public async Task UpdateAsync(string id, string sku, string name, string description, decimal price, int categoryId)
        {
            await _repo.UpdateAsync(id, sku, name, description, price, categoryId);
        }

        public async Task PatchIsActiveAsync(string id, bool isActive)
        {
            await _repo.PatchIsActiveAsync(id, isActive);
        }
    }
}
