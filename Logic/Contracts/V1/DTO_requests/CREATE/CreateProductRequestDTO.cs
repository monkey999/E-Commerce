using System;

namespace E_Commerce_Shop.Contracts.V1.DTO_requests.CREATE
{
    public class CreateProductRequestDTO
    {
        public Guid ProductId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Price { get; set; }
    }
}
