using A_Domain.Models;
using E_Commerce_Shop.Contracts.V1.DTO_responses.GET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Logic.Interfaces
{
    public interface IStockService
    {
        Task<bool> CreateStockAsync(Stock stock);
        Task<IEnumerable<GetStocksProductsResponseDTO>> GetStockAsync();
        Task<bool> UpdateStockAsync(Stock stock);
        Task<bool> UpdateRangeStockAsync(IEnumerable<Stock> stocks);
        Task<bool> DeleteStockByIdAsync(Guid stockId);
    }
}
