using B_DataAccess.Contracts.V1.DTO_requests.CREATE;
using B_DataAccess.Contracts.V1.DTO_responses.CREATE;
using C_Logic.Interfaces;
using E_Commerce_Shop.Contracts.V1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_Commerce_Shop.Controllers.V1
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "User")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerInfoService _customerInfoService;

        public CustomerController(ICustomerInfoService customerInfoService)
        {
            _customerInfoService = customerInfoService;
        }

        [HttpGet(ApiRoutes.CustomerInfo.GetCustomerInfo)]
        public async Task<IActionResult> GetCustomerInfoAsync()
        {
            var customerInformation = await _customerInfoService.GetCustomerInfoAsync();

            if (customerInformation == null)
                return NoContent();

            return Ok(customerInformation);
        }

        [HttpPost(ApiRoutes.CustomerInfo.AddCustomerInfo)]
        public IActionResult AddCustomerInfo([FromBody] CreateCustomerInfoRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //change later to better validation
            }

            _customerInfoService.AddCustomerInfo(request);

            var locationUri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}" + "/" + ApiRoutes.CustomerInfo.GetCustomerInfo;

            return Created(locationUri, new CreateCustomerInfoResponseDTO()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address1 = request.Address1,
                Address2 = request.Address2,
                City = request.City,
                Country = request.Country,
                PostalCode = request.PostalCode
            });
        }
    }
}
