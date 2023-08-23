using A_Domain.Models;
using C_Logic.Interfaces;
using E_Commerce_Shop.Contracts.V1;
using E_Commerce_Shop.Contracts.V1.DTO_requests.CREATE;
using E_Commerce_Shop.Contracts.V1.DTO_requests.UPDATE;
using E_Commerce_Shop.Contracts.V1.DTO_responses;
using E_Commerce_Shop.Contracts.V1.DTO_responses.UPDATE;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce_Shop.Controllers.V1
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "OnlyForManager")]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet(ApiRoutes.Stock.GetStock)]
        public async Task<IActionResult> GetStock()
        {
            var stock = await _stockService.GetStockAsync();

            return Ok(stock);
        }

        [HttpPost(ApiRoutes.Stock.AddStock)]
        public async Task<IActionResult> AddStock([FromBody] CreateStockRequestDTO request)
        {
            await _stockService.CreateStockAsync(new Stock()
            {
                StockId = request.StockId,
                Description = request.Description,
                Quantity = request.Quantity,
                ProductId = request.ProductId
            });

            var locationUri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}" + "/" + ApiRoutes.Stock.GetStock;

            return Created(locationUri, new CreateStockResponseDTO()
            {
                StockId = request.StockId,
                Description = request.Description,
                Quantity = request.Quantity
            });
        }

        [HttpDelete(ApiRoutes.Stock.DeleteStock)]
        public async Task<IActionResult> DeleteStock([FromRoute] Guid stockId)
        {
            var deleted = await _stockService.DeleteStockByIdAsync(stockId);

            if (deleted)
                return NoContent();

            return NotFound();
        }

        [HttpPut(ApiRoutes.Stock.UpdateStock)]
        public async Task<IActionResult> UpdateStock([FromBody] UpdateStockRequestDTO request)
        {
            var stocks = new List<Stock>();

            foreach (var stock in request.Stock)
            {
                stocks.Add(new Stock
                {
                    StockId = stock.StockId,
                    Description = stock.Description,
                    ProductId = stock.ProductId,
                    Quantity = stock.Quantity
                });
            }

            var updated = await _stockService.UpdateRangeStockAsync(stocks);

            if (updated)
            {
                var updatedStock = new UpdateStockResponseDTO()
                {
                    // TODO: automapper 
                    Stock = request.Stock.Select(x => new UpdateStockResponseDTO.UpdateStockModelDTO
                    {
                        StockId = x.StockId,
                        ProductId = x.ProductId,
                        Description = x.Description,
                        Quantity = x.Quantity
                    })
                };

                return Ok(updatedStock);
            }

            return NotFound();
        }
    }
}
