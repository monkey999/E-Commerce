namespace A_Domain.Models.Stripe
{
    public class CreateStripePaymentDTO
    {
        public string Description { get; set; }
        public string Currency { get; set; }

        public StripeCard StripeCard { get; set; }
    }

    public class StripeCard
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationYear { get; set; }
        public string ExpirationMonth { get; set; }
        public string Cvc { get; set; }
    }
}
