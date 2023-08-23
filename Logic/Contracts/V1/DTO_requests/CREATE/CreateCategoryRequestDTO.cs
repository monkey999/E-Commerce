using System;

namespace E_Commerce_Shop.Contracts.V1.DTO_requests.CREATE
{
    public class CreateCategoryRequestDTO
    {
        public Guid CategoryId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
