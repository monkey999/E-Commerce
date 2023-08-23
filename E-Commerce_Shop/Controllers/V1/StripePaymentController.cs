using A_Domain.Models.Stripe;
using C_Logic.Interfaces;
using E_Commerce_Shop.Contracts.V1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace E_Commerce_Shop.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    [ApiController]
    public class StripePaymentController : ControllerBase
    {
        private readonly IStripeAppService _stripeService;
        private readonly ICustomerInfoService _customerInfoService;

        public StripePaymentController(IStripeAppService stripeService, ICustomerInfoService customerInfoService)
        {
            _stripeService = stripeService;
            _customerInfoService = customerInfoService;
        }

        [HttpPost(ApiRoutes.Stripe.AddPayment)]
        public async Task<ActionResult<StripePaymentOrder>> AddStripePayment([FromBody] CreateStripePaymentDTO payment, CancellationToken ct)
        {
            if ((await _customerInfoService.GetCustomerInfoAsync()) == null)
                return NotFound();

            StripePaymentOrder createdPayment = await _stripeService.AddStripePaymentAsync(payment, ct);

            var locationUri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}" + "/" + ApiRoutes.Stripe.AddPayment;

            return Created(locationUri, createdPayment);
        }
    }
}

