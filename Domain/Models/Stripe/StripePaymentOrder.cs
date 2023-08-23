using A_Domain.ValueObjects;
using System;

namespace A_Domain.Models.Stripe
{
    public class StripePaymentOrder
    {
        public string CustomerId { get; set; }
        public string PaymentId { get; set; }
        public Guid OrderReference { get; set; }

        public string ReceiptEmail { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }

        public CustomerInformation CustomerInformation { get; set; }
    }
}
