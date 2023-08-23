namespace A_Domain.Models.Stripe
{
    public class StripeCustomer
    {
        public StripeCustomer(string name, string email, string customerId)
        {
            Name = name;
            Email = email;
            CustomerId = customerId;
        }

        public string CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
