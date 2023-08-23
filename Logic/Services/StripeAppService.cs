using A_Domain.Models.Stripe;
using C_Logic.Interfaces;
using E_Commerce_Shop.Contracts.V1.DTO_requests.CREATE;
using Logic.Services;
using Microsoft.AspNetCore.Http;
using Stripe;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace C_Logic.Services
{
    public class StripeAppService : IStripeAppService
    {
        private readonly ChargeService _chargeService;
        private readonly CustomerService _customerService;
        private readonly TokenService _tokenService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession Session => _httpContextAccessor.HttpContext.Session;

        public StripeAppService(ChargeService chargeService, CustomerService customerService, TokenService tokenService,
            IShoppingCartService shoppingCartService, IOrderService orderService, IHttpContextAccessor httpContextAccessor)
        {
            _chargeService = chargeService;
            _customerService = customerService;
            _tokenService = tokenService;
            _shoppingCartService = shoppingCartService;
            _orderService = orderService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<StripePaymentOrder> AddStripePaymentAsync(CreateStripePaymentDTO payment, CancellationToken ct)
        {
            var cartOrder = _shoppingCartService.GetOrderFromCart();

            TokenCreateOptions tokenOptions = new()
            {
                Card = new TokenCardOptions
                {
                    Name = payment.StripeCard.CardName,
                    Number = payment.StripeCard.CardNumber,
                    ExpYear = payment.StripeCard.ExpirationYear,
                    ExpMonth = payment.StripeCard.ExpirationMonth,
                    Cvc = payment.StripeCard.Cvc
                }
            };

            Token stripeToken = await _tokenService.CreateAsync(tokenOptions, null, ct);

            CustomerCreateOptions customerOptions = new()
            {
                Name = cartOrder.CustomerInformation.FirstName + " " + cartOrder.CustomerInformation.LastName,
                Email = cartOrder.CustomerInformation.Email,
                Source = stripeToken.Id
            };

            Customer createdCustomer = await _customerService.CreateAsync(customerOptions, null, ct);

            ChargeCreateOptions paymentOptions = new()
            {
                Customer = createdCustomer.Id,
                ReceiptEmail = cartOrder.CustomerInformation.Email,
                Description = payment.Description,
                Currency = payment.Currency,
                Amount = (int)(cartOrder.TotalCharge() * 100)
            };

            var createdPayment = await _chargeService.CreateAsync(paymentOptions, null, ct);

            var createOrder = new CreateOrderRequestDTO()
            {
                SessionId = Session.Id,
                StripeReference = createdPayment.Id,

                FirstName = cartOrder.CustomerInformation.FirstName,
                LastName = cartOrder.CustomerInformation.LastName,
                Email = cartOrder.CustomerInformation.Email,
                PhoneNumber = cartOrder.CustomerInformation.PhoneNumber,
                Address1 = cartOrder.CustomerInformation.Address1,
                Address2 = cartOrder.CustomerInformation.Address2,
                City = cartOrder.CustomerInformation.City,
                Country = cartOrder.CustomerInformation.Country,
                PostalCode = cartOrder.CustomerInformation.PostalCode,

                OrderStocks = cartOrder.ListOfCartItems.Select(x => new CreateOrderRequestDTO.OrderStockDTO()
                {
                    StockId = x.StockId,
                    Quantity = x.Quantity
                }).ToList()
            };

            await _orderService.CreateOrderAsync(createOrder);

            return new StripePaymentOrder()
            {
                CustomerId = createdCustomer.Id,
                PaymentId = createdPayment.Id,
                Amount = cartOrder.TotalCharge(),
                Currency = payment.Currency,
                CustomerInformation = cartOrder.CustomerInformation,
                Description = payment.Description,
                OrderReference = createOrder.OrderReference,
                ReceiptEmail = cartOrder.CustomerInformation.Email
            };
        }
    }
}
