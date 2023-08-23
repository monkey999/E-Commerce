using System;

namespace E_Commerce_Shop.Contracts.V1.DTO_requests.CREATE
{
    public class CreateCartItemRequestDTO
    {
        public Guid StockId { get; set; }
        public int Quantity { get; set; }
    }
}
