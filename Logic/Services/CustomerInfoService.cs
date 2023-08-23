using A_Domain.ValueObjects;
using B_DataAccess.Contracts.V1.DTO_requests.CREATE;
using B_DataAccess.Contracts.V1.DTO_responses.GET;
using C_Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class CustomerInfoService : ICustomerInfoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession Session => _httpContextAccessor.HttpContext.Session;

        public CustomerInfoService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddCustomerInfo(CreateCustomerInfoRequestDTO customerInfo)
        {
            var customerInformation = new CustomerInformation()
            {
                FirstName = customerInfo.FirstName,
                LastName = customerInfo.LastName,
                Email = customerInfo.Email,
                PhoneNumber = customerInfo.PhoneNumber,
                Address1 = customerInfo.Address1,
                Address2 = customerInfo.Address2,
                City = customerInfo.City,
                Country = customerInfo.Country,
                PostalCode = customerInfo.PostalCode
            };

            var stringObject = JsonSerializer.Serialize(customerInformation);

            Session.SetString("customer-info", stringObject);
        }

        public async Task<GetCustomerInfoResponseDTO> GetCustomerInfoAsync()
        {
            var stringObject = Session.GetString("customer-info");

            if (string.IsNullOrEmpty(stringObject))
                return null;

            MemoryStream stream = new(Encoding.UTF8.GetBytes(stringObject));

            var customerInformation = await JsonSerializer.DeserializeAsync<CustomerInformation>(stream);

            GetCustomerInfoResponseDTO response = new()
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
            };

            return response;
        }
    }
}
