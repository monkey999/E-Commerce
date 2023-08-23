using A_Domain.Models;
using A_Domain.Repo_interfaces;
using A_Domain.ValueObjects;
using B_DataAccess.Contracts.V1.DTO_responses.GET;
using E_Commerce_Shop.Contracts.V1.DTO_requests.CREATE;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGenericRepository<Stock, Guid> _stockRepository;
        private readonly IGenericRepository<StockOnHold, Guid> _stockOnHoldRepository;
        private ISession Session => _httpContextAccessor.HttpContext.Session;

        public ShoppingCartService(IHttpContextAccessor httpContextAccessor, IGenericRepository<Stock, Guid> stockRepository,
            IGenericRepository<StockOnHold, Guid> stockOnHoldRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _stockRepository = stockRepository;
            _stockOnHoldRepository = stockOnHoldRepository;
        }

        public async Task<bool> AddToCart(CreateCartItemRequestDTO cartItem) //change later to return task tuple(bool, string)
        {
            var stockToHold = await _stockRepository.FindByCondition(x => x.StockId == cartItem.StockId).FirstOrDefaultAsync();
            var stockOnHold = await _stockOnHoldRepository.FindByCondition(x => x.SessionId == Session.Id).ToListAsync();
            
            if (stockToHold.Quantity < cartItem.Quantity)
            {
                return false;
            }
           
            if (stockOnHold.Any(x => x.StockId == cartItem.StockId)) 
            {
                stockOnHold.Find(x => x.StockId == cartItem.StockId).Quantity += cartItem.Quantity;
            }
            
            else
            {
                await _stockOnHoldRepository.AddAsync(new StockOnHold()
                {
                    StockId = cartItem.StockId,
                    SessionId = Session.Id,
                    Quantity = cartItem.Quantity,
                    ExpiryDate = DateTime.Now.AddMinutes(20)
                });
            }

            stockToHold.Quantity -= cartItem.Quantity;

            foreach (var stock in stockOnHold)
            {
                stock.ExpiryDate = DateTime.Now.AddMinutes(20);
            }

            _stockRepository.Update(stockToHold);
            _stockOnHoldRepository.UpdateRange(stockOnHold);

            await _stockOnHoldRepository.SaveChangesAsync(); // implement unitofwork pattern 
            await _stockRepository.SaveChangesAsync();

            var cartItemList = new List<CartItem>();
            var stringObject = Session.GetString("cart");

            if (!string.IsNullOrEmpty(stringObject))
            {
                cartItemList = JsonSerializer.Deserialize<List<CartItem>>(stringObject);
            }

            if (cartItemList.Any(x => x.StockId == cartItem.StockId))
            {
                cartItemList.Find(x => x.StockId == cartItem.StockId).Quantity += cartItem.Quantity;
            }

            else
            {
                cartItemList.Add(new CartItem
                {
                    StockId = cartItem.StockId,
                    Quantity = cartItem.Quantity
                });
            }

            stringObject = JsonSerializer.Serialize(cartItemList);

            Session.SetString("cart", stringObject);

            return true;
        }

        public IEnumerable<GetCartResponseDTO> GetCart()
        {
            var stringObject = Session.GetString("cart");

            if (string.IsNullOrEmpty(stringObject))
            {
                return new List<GetCartResponseDTO>();
            }

            var cartItemList = JsonSerializer.Deserialize<List<CartItem>>(stringObject);

            var response = _stockRepository
                .GetAll()
                .Include(x => x.Product)
                .AsEnumerable()
                .Where(x => cartItemList.Any(y => y.StockId == x.StockId))
                .Select(x => new GetCartResponseDTO()
                {
                    ProductName = x.Product.Name,
                    Price = $"$ {x.Product.Price:N2}",
                    StockId = x.StockId,
                    Quantity = cartItemList.FirstOrDefault(y => y.StockId == x.StockId).Quantity
                })
                .ToList();

            return response;
        }

        public GetOrderFromCartResponseDTO GetOrderFromCart()
        {
            var cartItemList = JsonSerializer.Deserialize<List<CartItem>>(Session.GetString("cart"));

            var listOfProducts = _stockRepository
                    .GetAll()
                    .Include(x => x.Product)
                    .AsEnumerable()
                    .Where(x => cartItemList.Any(y => y.StockId == x.StockId))
                    .Select(x => new CartOrderProduct()
                    {
                        ProductId = x.ProductId,
                        StockId = x.StockId,
                        Price = x.Product.Price,
                        Quantity = cartItemList.FirstOrDefault(y => y.StockId == x.StockId).Quantity
                    }).ToList();

            var customerInformation = JsonSerializer.Deserialize<CustomerInformation>(Session.GetString("customer-info"));

            return new GetOrderFromCartResponseDTO()
            {
                ListOfCartItems = listOfProducts,
                CustomerInformation = new()
                {
                    FirstName = customerInformation.FirstName,
                    LastName = customerInformation.LastName,
                    Email = customerInformation.Email,
                    PhoneNumber = customerInformation.PhoneNumber,
                    Address1 = customerInformation.Address1,
                    Address2 = customerInformation.Address2,
                    City = customerInformation.City,
                    Country = customerInformation.Country,
                    PostalCode = customerInformation.PostalCode
                }
            };
        }
    }
}
