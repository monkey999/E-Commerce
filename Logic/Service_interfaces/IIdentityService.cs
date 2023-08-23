using A_Domain.Models;
using E_Commerce_Shop.Contracts.V1.DTO_requests.CREATE;
using System.Threading.Tasks;

namespace C_Logic.Interfaces
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(UserRegistrationRequest request);
        Task<AuthenticationResult> LoginAsync(string email, string password);
        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
        Task Logout();
    }
}
