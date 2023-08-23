using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce_Shop.Contracts.V1.DTO_responses
{
    public class CreateStockResponseDTO
    {
        public Guid StockId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }
}
