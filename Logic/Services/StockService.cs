using A_Domain.Models;
using A_Domain.Repo_interfaces;
using C_Logic.Interfaces;
using E_Commerce_Shop.Contracts.V1.DTO_responses.GET;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace C_Logic.Services
{
    public class StockService : IStockService
    {
        //private readonly IGenericRepository<Stock, Guid> _stockRepository;
        //private readonly IGenericRepository<Product, Guid> _productRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IProductRepository _productRepository;

        public StockService(IStockRepository stockRepository, IProductRepository productRepository)
        {
            _stockRepository = stockRepository;
            _productRepository = productRepository;
        }

        public async Task<bool> CreateStockAsync(Stock stock)
        {
            await _stockRepository.AddAsync(stock);

            var created = await _stockRepository.SaveChangesAsyncWithResult();

            return created > 0;
        }

        public async Task<bool> DeleteStockByIdAsync(Guid stockId)
        {
            var stock = await _stockRepository.FindByCondition(x => x.StockId == stockId).SingleOrDefaultAsync();

            if (stock == null)
                return false;

            _stockRepository.RemoveById(stockId);

            var deleted = await _stockRepository.SaveChangesAsyncWithResult();

            return deleted > 0;
        }

        public async Task<IEnumerable<GetStocksProductsResponseDTO>> GetStockAsync()
        {
            var stock = await _productRepository.GetAll()
                .Include(x => x.Stock)
                .Select(x => new GetStocksProductsResponseDTO()
                {
                    ProductId = x.ProductId,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    CategoryId = x.CategoryId,
                    Stock = x.Stock.Select(y => new StockModelDTO()
                    {
                        StockId = y.StockId,
                        Description = y.Description,
                        Quantity = y.Quantity
                    })
                })
                .ToListAsync();

            return stock;
        }

        public async Task<bool> UpdateRangeStockAsync(IEnumerable<Stock> stocks)
        {
            _stockRepository.UpdateRange(stocks);

            var updated = await _stockRepository.SaveChangesAsyncWithResult();

            return updated > 0;
        }

        public async Task<bool> UpdateStockAsync(Stock stock)
        {
            _stockRepository.Update(stock);

            var updated = await _stockRepository.SaveChangesAsyncWithResult();

            return updated > 0;
        }
    }
}
