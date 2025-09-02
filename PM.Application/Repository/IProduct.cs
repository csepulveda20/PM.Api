using PM.Application.Common;
using PM.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PM.Application.Repository
{
    public interface IProduct
    {
        Task<IEnumerable<Product>> ListAsync(QueryParam queryParam, int? categoryId = null, bool? isActive = null);
        Task<Product> GetByIdAsync(string id);
        Task CreateAsync(Product product);
        Task UpdateAsync(string id, string sku, string name, string description, decimal price, int categoryId);
        Task PatchIsActiveAsync(string id, bool isActive);
    }
}
