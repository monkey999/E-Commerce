using A_Domain.Models;
using E_Commerce_Shop.Contracts.V1;
using E_Commerce_Shop.Contracts.V1.DTO_requests;
using E_Commerce_Shop.Contracts.V1.DTO_requests.CREATE;
using E_Commerce_Shop.Contracts.V1.DTO_responses;
using Logic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace E_Commerce_Shop.Controllers.V1
{
    [ApiController]
    [Authorize(Policy = "Manager")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost(ApiRoutes.Categories.AddCategory)]
        public async Task<IActionResult> AddCategory([FromBody] CreateCategoryRequestDTO request)
        {
            await _categoryService.CreateCategoryAsync(new Category()
            {
                CategoryId = request.CategoryId,
                Name = request.Name,
                Description = request.Description
            });

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";

            var locationUri = baseUrl + "/" + ApiRoutes.Categories.GetCategoryByID.Replace("{categoryId}", request.CategoryId.ToString());

            return Created(locationUri, new CreateCategoryResponseDTO() { Id = request.CategoryId });
        }

        [HttpGet(ApiRoutes.Categories.GetAllCategories)]
        public async Task<IActionResult> GetAllCategories()
        {
            return Ok(await _categoryService.GetAllCategoriesAsync());
        }

        [HttpGet(ApiRoutes.Categories.GetCategoryByID)]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid categoryId)
        {
            var category = await _categoryService.GetCategoryByIdAsync(categoryId);

            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPut(ApiRoutes.Categories.UpdateCategory)]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid categoryId, [FromBody] UpdateCategoryRequestDTO request)
        {
            var category = new Category
            {
                CategoryId = categoryId,
                Name = request.Name,
                Description = request.Description
            };

            var updated = await _categoryService.UpdateCategoryAsync(category);

            if (updated)
                return Ok(category);

            return NotFound();
        }

        [HttpDelete(ApiRoutes.Categories.DeleteCategory)]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid categoryId)
        {
            var deleted = await _categoryService.DeleteCategoryAsync(categoryId);

            if (deleted)
                return NoContent();

            return NotFound();
        }
    }
}
