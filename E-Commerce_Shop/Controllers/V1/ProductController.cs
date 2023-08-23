using A_Domain.Models;
using E_Commerce_Shop.Contracts.V1;
using E_Commerce_Shop.Contracts.V1.DTO_requests.CREATE;
using E_Commerce_Shop.Contracts.V1.DTO_requests.UPDATE;
using E_Commerce_Shop.Contracts.V1.DTO_responses;
using Logic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce_Shop.Controllers.V1
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet(ApiRoutes.Products.GetAllProducts)]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetProductsAsync();

            return Ok(products);
        }

        [HttpGet(ApiRoutes.Products.GetProductByID)]
        public async Task<IActionResult> GetProductById([FromRoute] Guid productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);

            if (product.Stocks.All(x => !x.IsInStock))
            {
                return NotFound("Product is out of stock!");
            }

            return Ok(product);
        }

        [HttpPost(ApiRoutes.Products.AddProduct)]
        [Authorize(Policy = "OnlyForManager")]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductRequestDTO request)
        {
            await _productService.CreateProductAsync(new Product()
            {
                ProductId = request.ProductId,
                Name = request.Name,
                Description = request.Description,
                CategoryId = request.CategoryId,
                Price = request.Price
            });

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";

            var locationUri = baseUrl + "/" + ApiRoutes.Products.GetProductByID
                .Replace("{productId}", request.ProductId.ToString());

            return Created(locationUri, new CreateProductResponseDTO() { Id = request.ProductId });
        }

        [HttpPut(ApiRoutes.Products.UpdateProduct)]
        [Authorize(Policy = "OnlyForManager")]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid productId, [FromBody] UpdateProductRequestDTO request)
        {
            var product = new Product()
            {
                ProductId = productId,
                Name = request.Name,
                Description = request.Description,
                CategoryId = request.CategoryId,
                Price = request.Price
            };

            var updated = await _productService.UpdateProductAsync(product);

            if (updated)
                return Ok(product);

            return NotFound();
        }

        [HttpDelete(ApiRoutes.Products.DeleteProduct)]
        [Authorize(Policy = "OnlyForManager")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid productId)
        {
            var deleted = await _productService.DeleteProductAsync(productId);

            if (deleted)
                return NoContent();

            return NotFound();
        }
    }
}

