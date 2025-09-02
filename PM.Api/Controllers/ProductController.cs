using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PM.Application.Common;
using PM.Application.UseCases.Product;
using PM.Domain.Entities;

namespace PM.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/admin/products")]
    [Authorize(Roles = "admin")]
    public class ProductController : ControllerBase
    {
        private readonly ProductUseCase _useCase;
        public ProductController(ProductUseCase useCase)
        {
            _useCase = useCase;
        }

        // 1. GET - admin/products (con filtros y paginación en el body)
        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] ProductSearchRequest request)
        {
            var queryParam = new QueryParam
            {
                Search = request.Search,
                OrderBy = request.Sort,
                PageNumber = request.Page,
                PageSize = request.PageSize
            };
            var result = await _useCase.ListAsync(queryParam, request.CategoryId, request.IsActive);
            Response.Headers["X-Total-Count"] = result.Total.ToString();
            return Ok(result);
        }

        // 2. POST - /admin/products
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateRequest request)
        {
            // Validar SKU único
            var existing = await _useCase.ListAsync(new QueryParam { Search = request.Sku, PageNumber = 1, PageSize = 1 });
            if (existing.Items != null && System.Linq.Enumerable.Any(existing.Items, p => p.Sku == request.Sku))
                return Conflict(new { message = "SKU duplicado" });
            var product = new Product
            {
                Sku = request.Sku,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                CategoryId = request.CategoryId,
                IsActive = request.IsActive ?? true,
                CreatedAt = System.DateTime.UtcNow
            };
            await _useCase.CreateAsync(product);
            return StatusCode(StatusCodes.Status201Created, product);
        }

        // 3. GET - admin/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var product = await _useCase.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        // 4. PUT - admin/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ProductUpdateRequest request)
        {
            await _useCase.UpdateAsync(id, request.Sku, request.Name, request.Description, request.Price, request.CategoryId);
            return NoContent();
        }

        // 5. PATCH - admin/products/{id}/toggle
        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> Toggle(string id, [FromBody] ToggleRequest request)
        {
            await _useCase.PatchIsActiveAsync(id, request.IsActive);
            return NoContent();
        }
    }

    public class ProductSearchRequest
    {
        public string Search { get; set; }
        public string? CategoryId { get; set; }
        public bool? IsActive { get; set; }
        public string Sort { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    public class ProductCreateRequest
    {
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string? CategoryId { get; set; }
        public bool? IsActive { get; set; }
    }
    public class ProductUpdateRequest
    {
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryId { get; set; }
    }
    public class ToggleRequest
    {
        public bool IsActive { get; set; }
    }
}
