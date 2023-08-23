using A_Domain.Models;
using B_DataAccess.Contracts.V1.DTO_requests.UPDATE;
using C_Logic.Extensions;
using E_Commerce_Shop.Contracts.V1;
using E_Commerce_Shop.Contracts.V1.DTO_requests.CREATE;
using E_Commerce_Shop.Contracts.V1.DTO_responses;
using Logic.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce_Shop.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet(ApiRoutes.Orders.GetAllOrders)]
        public async Task<IActionResult> GetAllOrders()
        {
            return Ok(await _orderService.GetOrdersAsync());
        }

        [HttpPost(ApiRoutes.Orders.CreateOrder)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequestDTO request)
        {
            await _orderService.CreateOrderAsync(request);

            var locationUri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}"
                                    + "/" + ApiRoutes.Orders.GetOrderByReference;

            return Created(locationUri, new CreateOrderResponseDTO()
            {
                OrderId = request.OrderId,

                SessionId = request.SessionId,
                StripeReference = request.StripeReference,
                OrderReference = request.OrderReference,

                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address1 = request.Address1,
                Address2 = request.Address2,
                City = request.City,
                Country = request.Country,
                PostalCode = request.PostalCode,

                OrderStocks = request.OrderStocks.Select(x => new CreateOrderResponseDTO.OrderStockDTO()
                {
                    StockId = x.StockId,
                    Quantity = x.Quantity
                }).ToList()
            });
        }

        [HttpGet(ApiRoutes.Orders.GetOrderByID)]
        public async Task<IActionResult> GetOrderById([FromRoute] Guid orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpGet(ApiRoutes.Orders.GetOrderByReference)]
        public async Task<IActionResult> GetOrderByReference([FromRoute] Guid orderReference)
        {
            var order = await _orderService.GetOrderByReferenceAsync(orderReference);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPut(ApiRoutes.Orders.UpdateOrder)]
        public async Task<IActionResult> UpdateOrder([FromRoute] Guid orderId, [FromBody] UpdateOrderRequestDTO request)
        {
            var userOwnsOrder = await _orderService.UserOwnsOrderAsync(orderId, HttpContext.GetUserId());

            if (!userOwnsOrder)
            {
                return BadRequest(error: new { error = "You do not own this order!" });
            }

            var order = new Order
            {
                OrderId = orderId,
                Address1 = request.Address1,
                Address2 = request.Address2,
                City = request.City,
                Country = request.Country,
                PostalCode = request.PostalCode
            };

            var updated = await _orderService.UpdateOrderAsync(order);

            if (updated)
                return Ok(order);

            return NotFound();
        }

        [HttpDelete(ApiRoutes.Orders.DeleteOrder)]
        public async Task<IActionResult> DeleteOrder([FromRoute] Guid orderId)
        {
            var userOwnsOrder = await _orderService.UserOwnsOrderAsync(orderId, HttpContext.GetUserId());

            if (!userOwnsOrder)
            {
                return BadRequest(error: new { error = "You do not own this order!" });
            }

            var deleted = await _orderService.DeleteOrderAsync(orderId);

            if (deleted)
                return NoContent();

            return NotFound();
        }
    }
}
