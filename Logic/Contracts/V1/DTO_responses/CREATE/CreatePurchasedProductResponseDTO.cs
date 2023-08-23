using System;

namespace E_Commerce_Shop.Contracts.V1.DTO_responses
{
    public class CreatePurchasedProductResponseDTO
    {
        public Guid Id { get; set; }
        public DateTime? DatePurchased { get; set; }
    }
}
