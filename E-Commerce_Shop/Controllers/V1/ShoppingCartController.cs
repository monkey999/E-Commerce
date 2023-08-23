using B_DataAccess.Contracts.V1.DTO_responses.CREATE;
using E_Commerce_Shop.Contracts.V1;
using E_Commerce_Shop.Contracts.V1.DTO_requests.CREATE;
using Logic.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_Commerce_Shop.Controllers.V1
{
    [AllowAnonymous]
    [Route("[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpPost(ApiRoutes.ShoppingCart.AddToCart)]
        public async Task<IActionResult> AddToCart([FromBody] CreateCartItemRequestDTO cartItem)
        {
            var locationUri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}" + "/" + ApiRoutes.ShoppingCart.GetCart;

            if (await _shoppingCartService.AddToCart(cartItem))
            {
                return Created(locationUri, new CreateCartItemResponseDTO()
                {
                    StockId = cartItem.StockId,
                    Quantity = cartItem.Quantity
                });
            }

            else
            {
                return BadRequest();
            }
        }

        [HttpGet(ApiRoutes.ShoppingCart.GetCart)]
        public IActionResult GetCart()
        {
            return Ok(_shoppingCartService.GetCart());
        }
    }
}
