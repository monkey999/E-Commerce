using A_Domain.Models;
using B_DataAccess.Contracts.V1.DTO_responses.GET;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IProductService
    {
        Task<bool> CreateProductAsync(Product product);
        Task<IEnumerable<GetProductResponseDTO>> GetProductsAsync();
        Task<GetProductResponseDTO> GetProductByIdAsync(Guid productId);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(Guid productId);
    }
}