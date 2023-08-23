using A_Domain.Models;
using A_Domain.Repo_interfaces;
using B_DataAccess.Contracts.V1.DTO_responses.GET;
using E_Commerce_Shop.Contracts.V1.DTO_requests.CREATE;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order, Guid> _orderRepository;
        private readonly IGenericRepository<StockOnHold, Guid> _stockOnHoldRepository;

        public OrderService(IGenericRepository<Order, Guid> orderRepository,
            IGenericRepository<StockOnHold, Guid> stockOnHoldRepository)
        {
            _orderRepository = orderRepository;
            _stockOnHoldRepository = stockOnHoldRepository;
        }

        public async Task<bool> CreateOrderAsync(CreateOrderRequestDTO order)
        {
            var stockOnHold = await _stockOnHoldRepository.FindByCondition(x => x.SessionId == order.SessionId).ToListAsync();
            _stockOnHoldRepository.RemoveRange(stockOnHold);
            await _stockOnHoldRepository.SaveChangesAsync();

            await _orderRepository.AddAsync(new Order()
            {
                OrderId = order.OrderId,

                SessionId = order.SessionId,
                StripeReference = order.StripeReference,
                OrderReference = order.OrderReference,

                FirstName = order.FirstName,
                LastName = order.LastName,
                Email = order.Email,
                PhoneNumber = order.PhoneNumber,
                Address1 = order.Address1,
                Address2 = order.Address2,
                City = order.City,
                Country = order.Country,
                PostalCode = order.PostalCode,

                OrderStocks = order.OrderStocks.Select(x => new OrderStock()
                {
                    StockId = x.StockId,
                    Quantity = x.Quantity
                }).ToList()
            });

            return await _orderRepository.SaveChangesAsyncWithResult() > 0;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _orderRepository.GetAll()
                            .OrderBy(u => u.OrderId)
                                .ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await _orderRepository.FindByCondition(u => u.OrderId.Equals(orderId))
                            .SingleOrDefaultAsync();
        }

        public async Task<GetOrderByRefDTO> GetOrderByReferenceAsync(Guid orderReference)
        {
            return await _orderRepository.FindByCondition(x => x.OrderReference == orderReference)
                            .Include(x => x.OrderStocks)
                            .ThenInclude(x => x.Stock)
                            .ThenInclude(x => x.Product)
                            .Select(x => new GetOrderByRefDTO
                            {
                                OrderReference = x.OrderReference,

                                FirstName = x.FirstName,
                                LastName = x.LastName,
                                Email = x.Email,
                                PhoneNumber = x.PhoneNumber,
                                Address1 = x.Address1,
                                Address2 = x.Address2,
                                City = x.City,
                                Country = x.Country,
                                PostalCode = x.PostalCode,
                                Products = x.OrderStocks.Select(y => new GetOrderByRefDTO.Product()
                                {
                                    Name = y.Stock.Product.Name,
                                    Description = y.Stock.Product.Description,
                                    Price = y.Stock.Product.Price.ToString(),
                                    Quantity = y.Quantity.ToString(),
                                    StockDescription = y.Stock.Description
                                }),
                                TotalValue = x.OrderStocks.Sum(y => y.Stock.Product.Price).ToString()
                            })
                            .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            _orderRepository.Update(order);

            return await _orderRepository.SaveChangesAsyncWithResult() > 0;
        }

        public async Task<bool> DeleteOrderAsync(Guid orderId)
        {
            var user = await GetOrderByIdAsync(orderId);

            if (user == null)
                return false;

            _orderRepository.RemoveById(orderId);

            return await _orderRepository.SaveChangesAsyncWithResult() > 0;
        }

        public async Task<bool> UserOwnsOrderAsync(Guid orderId, string userId)
        {
            var order = await _orderRepository.GetAll().SingleOrDefaultAsync(x => x.OrderId == orderId);

            if (order == null)
                return false;

            //if (order.UserId != userId)
            //    return false;

            return true;
        }
    }
}
