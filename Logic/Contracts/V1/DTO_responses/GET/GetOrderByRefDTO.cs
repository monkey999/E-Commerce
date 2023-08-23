using System;
using System.Collections.Generic;

namespace B_DataAccess.Contracts.V1.DTO_responses.GET
{
    public class GetOrderByRefDTO
    {
        public Guid OrderReference { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public string TotalValue { get; set; }

        public class Product
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Price { get; set; }
            public string Quantity { get; set; }
            public string StockDescription { get; set; }
        }
    }
}
