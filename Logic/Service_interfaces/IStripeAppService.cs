using A_Domain.Models.Stripe;
using System.Threading;
using System.Threading.Tasks;

namespace C_Logic.Interfaces
{
    public interface IStripeAppService
    {
        Task<StripePaymentOrder> AddStripePaymentAsync(CreateStripePaymentDTO payment, CancellationToken ct);
    }
}
