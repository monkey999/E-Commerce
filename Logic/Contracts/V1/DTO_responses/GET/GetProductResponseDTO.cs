using System;
using System.Collections.Generic;

namespace B_DataAccess.Contracts.V1.DTO_responses.GET
{
    public class GetProductResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }

        public IEnumerable<StockDTOModel> Stocks { get; set; }
        public CategoryDTOModel Category { get; set; }
        public class CategoryDTOModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
        public class StockDTOModel
        {
            public Guid StockId { get; set; }
            public string Description { get; set; }
            public bool IsInStock { get; set; }
            public int Quantity { get; set; }
        }
    }
}
