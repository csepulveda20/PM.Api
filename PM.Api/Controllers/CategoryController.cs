using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PM.Application.UseCases.Category;
using PM.Domain.Entities;
using System.Threading.Tasks;

namespace PM.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/admin/categories")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryUseCase _useCase;
        public CategoryController(CategoryUseCase useCase)
        {
            _useCase = useCase;
        }

        // 1. GET - /admin/categories?page=1&pageSize=10&search=...&isActive=...
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = null, [FromQuery] bool? isActive = null)
        {
            var result = await _useCase.ListAsync(page, pageSize, search, isActive);
            Response.Headers["X-Total-Count"] = result.Total.ToString();
            return Ok(result);
        }

        // 2. POST - /admin/categories
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreateRequest request)
        {
            var category = new Category
            {
                Name = request.Name,
                ParentCategoryId = request.ParentCategoryId ?? 0,
                IsActive = request.IsActive ?? true,
                CreatedAt = System.DateTime.UtcNow
            };
            await _useCase.CreateAsync(category);
            return StatusCode(201, category);
        }

        // 3. PUT - /admin/categories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateRequest request)
        {
            await _useCase.UpdateAsync(id, request.Name, request.ParentCategoryId ?? 0);
            if (request.IsActive.HasValue)
                await _useCase.PatchIsActiveAsync(id, request.IsActive.Value);
            return NoContent();
        }

        // 4. PATCH - /admin/categories/{id}/toggle
        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> Toggle(int id, [FromBody] ToggleRequest request)
        {
            await _useCase.PatchIsActiveAsync(id, request.IsActive);
            return NoContent();
        }
    }

    public class CategoryCreateRequest
    {
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public bool? IsActive { get; set; }
    }
    public class CategoryUpdateRequest
    {
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public bool? IsActive { get; set; }
    }
}
