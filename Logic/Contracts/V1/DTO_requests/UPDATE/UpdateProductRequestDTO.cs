using System;

namespace E_Commerce_Shop.Contracts.V1.DTO_requests.UPDATE
{
    public class UpdateProductRequestDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Price { get; set; }
    }
}
