using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce_Shop.Contracts.V1.DTO_responses.GET
{
    public class GetStocksProductsResponseDTO
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Price { get; set; }

        public IEnumerable<StockModelDTO> Stock { get; set; }
    }

    public class StockModelDTO
    {
        public Guid StockId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }
}
