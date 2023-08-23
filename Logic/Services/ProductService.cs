using A_Domain.Models;
using A_Domain.Repo_interfaces;
using B_DataAccess.Contracts.V1.DTO_responses.GET;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product, Guid> _productRepository;
        private readonly IGenericRepository<StockOnHold, Guid> _stockOnHoldRepository;
        private readonly IGenericRepository<Stock, Guid> _stockRepository; // TODO: unit of work 

        public ProductService(IGenericRepository<Product, Guid> productRepository, IGenericRepository<StockOnHold, Guid> stockOnHoldRepository,
            IGenericRepository<Stock, Guid> stockRepository)
        {
            _productRepository = productRepository;
            _stockOnHoldRepository = stockOnHoldRepository;
            _stockRepository = stockRepository;
        }

        public async Task<bool> CreateProductAsync(Product product)
        {
            await _productRepository.AddAsync(product);

            var created = await _productRepository.SaveChangesAsyncWithResult();

            return created > 0;
        }

        public async Task<IEnumerable<GetProductResponseDTO>> GetProductsAsync()
        {
            return await _productRepository.GetAll()
                    .Include(x => x.Category)
                        .Include(x => x.Stock)
                            .Select(x => new GetProductResponseDTO()
                            {
                                Id = x.ProductId,
                                Name = x.Name,
                                Description = x.Description,
                                Price = $"$ {x.Price}",
                                Category = new GetProductResponseDTO.CategoryDTOModel()
                                {
                                    Id = x.Category.CategoryId,
                                    Name = x.Category.Name,
                                    Description = x.Category.Description
                                },
                                Stocks = x.Stock.Select(y => new GetProductResponseDTO.StockDTOModel()
                                {
                                    StockId = y.StockId,
                                    Description = y.Description,
                                    IsInStock = y.Quantity > 0,
                                    Quantity = x.Stock.Sum(y => y.Quantity)
                                })
                            }).ToListAsync();
        }

        public async Task<GetProductResponseDTO> GetProductByIdAsync(Guid productId)
        {
            var expiredStocksOnHold = _stockOnHoldRepository.FindByCondition(x => x.ExpiryDate < DateTime.Now);

            if (expiredStocksOnHold.AsEnumerable().Any())
            {
                var stockToReturn = await _stockRepository.FindByCondition(x => expiredStocksOnHold.Any(y => y.StockId == x.StockId)).ToListAsync();

                foreach (var stock in stockToReturn)
                {
                    stock.Quantity += expiredStocksOnHold.FirstOrDefault(x => x.StockId == stock.StockId).Quantity;
                }

                _stockRepository.UpdateRange(stockToReturn);
                _stockOnHoldRepository.RemoveRange(expiredStocksOnHold);

                await _stockOnHoldRepository.SaveChangesAsync(); // unitofwork pattern 
                await _stockRepository.SaveChangesAsync();
            }

            return await _productRepository.FindByCondition(u => u.ProductId.Equals(productId))
                     .Include(u => u.Category)
                        .Include(x => x.Stock)
                            .Select(x => new GetProductResponseDTO()
                            {
                                Id = x.ProductId,
                                Name = x.Name,
                                Description = x.Description,
                                Price = $"$ {x.Price}",
                                Category = new GetProductResponseDTO.CategoryDTOModel()
                                {
                                    Id = x.Category.CategoryId,
                                    Name = x.Category.Name,
                                    Description = x.Category.Description
                                },
                                Stocks = x.Stock.Select(y => new GetProductResponseDTO.StockDTOModel()
                                {
                                    StockId = y.StockId,
                                    Description = y.Description,
                                    IsInStock = y.Quantity > 0,
                                    Quantity = x.Stock.Sum(y => y.Quantity)
                                })
                            }).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            _productRepository.Update(product);

            var updated = await _productRepository.SaveChangesAsyncWithResult();

            return updated > 0;
        }

        public async Task<bool> DeleteProductAsync(Guid productId)
        {
            var product = await GetProductByIdAsync(productId);

            if (product == null)
                return false;

            _productRepository.RemoveById(productId);

            var deleted = await _productRepository.SaveChangesAsyncWithResult();

            return deleted > 0;
        }
    }
}
