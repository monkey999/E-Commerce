using System;
using System.Collections.Generic;

namespace A_Domain.Models
{
    public class Order
    {
        public Order()
        {
            OrderStocks = new HashSet<OrderStock>();
        }

        public Guid OrderId { get; set; }

        public Guid OrderReference { get; set; }
        public string SessionId { get; set; }
        public string StripeReference { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }

        public ICollection<OrderStock> OrderStocks { get; set; }
    }
}
