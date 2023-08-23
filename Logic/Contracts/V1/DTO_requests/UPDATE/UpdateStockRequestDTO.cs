using System;
using System.Collections.Generic;

namespace E_Commerce_Shop.Contracts.V1.DTO_requests.UPDATE
{
    public class UpdateStockRequestDTO
    {
        public IEnumerable<UpdateStockModelDTO> Stock { get; set; }
        public class UpdateStockModelDTO
        {
            public Guid StockId { get; set; }
            public Guid ProductId { get; set; }
            public string Description { get; set; }
            public int Quantity { get; set; }
        }
    }
}
