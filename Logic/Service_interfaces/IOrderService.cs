using A_Domain.Models;
using B_DataAccess.Contracts.V1.DTO_responses.GET;
using E_Commerce_Shop.Contracts.V1.DTO_requests.CREATE;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IOrderService
    {
        Task<bool> CreateOrderAsync(CreateOrderRequestDTO order);
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<Order> GetOrderByIdAsync(Guid orderId);
        Task<GetOrderByRefDTO> GetOrderByReferenceAsync(Guid orderReference);
        Task<bool> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(Guid orderId);
        Task<bool> UserOwnsOrderAsync(Guid orderId, string userId);
    }
}