using System;
using System.Collections.Generic;

namespace E_Commerce_Shop.Contracts.V1.DTO_requests.CREATE
{
    public class CreateOrderRequestDTO
    {
        public class OrderStockDTO
        {
            public Guid StockId { get; set; }
            public int Quantity { get; set; }
        }

        public Guid OrderId { get; set; } = Guid.NewGuid();
        public Guid OrderReference { get; set; } = Guid.NewGuid();
        public string StripeReference { get; set; }
        public string SessionId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }

        public List<OrderStockDTO> OrderStocks { get; set; }
    }
}
