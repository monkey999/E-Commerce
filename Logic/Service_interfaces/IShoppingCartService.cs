using B_DataAccess.Contracts.V1.DTO_responses.GET;
using E_Commerce_Shop.Contracts.V1.DTO_requests.CREATE;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IShoppingCartService
    {
        Task<bool> AddToCart(CreateCartItemRequestDTO cartItem);
        IEnumerable<GetCartResponseDTO> GetCart();
        GetOrderFromCartResponseDTO GetOrderFromCart();
    }
}
