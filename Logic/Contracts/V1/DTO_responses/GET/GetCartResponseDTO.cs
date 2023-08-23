using System;

namespace B_DataAccess.Contracts.V1.DTO_responses.GET
{
    public class GetCartResponseDTO
    {
        public Guid StockId { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        public int Quantity { get; set; }
    }
}
