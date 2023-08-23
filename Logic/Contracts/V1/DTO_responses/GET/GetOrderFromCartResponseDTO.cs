using A_Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace B_DataAccess.Contracts.V1.DTO_responses.GET
{
    public class GetOrderFromCartResponseDTO
    {
        public IEnumerable<CartOrderProduct> ListOfCartItems { get; set; }
        public CustomerInformation CustomerInformation { get; set; }
        public decimal TotalCharge() => ListOfCartItems.Sum(x => x.Price * x.Quantity);
    }

    public class CartOrderProduct
    {
        public Guid ProductId { get; set; }
        public Guid StockId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
