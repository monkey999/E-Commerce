using A_Domain.Models;
using E_Commerce_Shop.Contracts.V1;
using E_Commerce_Shop.Contracts.V1.DTO_requests;
using E_Commerce_Shop.Contracts.V1.DTO_requests.CREATE;
using E_Commerce_Shop.Contracts.V1.DTO_responses;
using Logic.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace E_Commerce_Shop.Controllers.V1
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "OnlyForManager")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet(ApiRoutes.Users.GetAllUsers)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetUsersAsync();

            return Ok(users);
        }

        [HttpGet(ApiRoutes.Users.GetLoggedInUser)]
        public async Task<IActionResult> GetLoggedInUser()
        {
            var user = await _userService.GetUserAsync();

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet(ApiRoutes.Users.GetUserByID)]
        public async Task<IActionResult> GetUser([FromRoute] Guid userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost(ApiRoutes.Users.CreateUser)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDTO request)
        {
            var result = await _userService.CreateUserAsync(request);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var locationUri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}" + "/" + ApiRoutes.Users.GetUserByID
                .Replace("{userId}", request.UserId.ToString());

            return Created(locationUri, new CreateUserResponseDTO() { Id = request.UserId.ToString() });
        }

        [HttpPut(ApiRoutes.Users.UpdateUser)]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid userId, [FromBody] UpdateUserRequestDTO request)
        {
            var user = new UserIdentity
            {
                Id = userId.ToString(),
                Name = request.Name,
                Surname = request.Surname,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Address = request.Adress
            };

            var updated = await _userService.UpdateUserAsync(user);

            if (updated.Succeeded)
                return Ok(user);

            return NotFound();
        }

        [HttpDelete(ApiRoutes.Users.DeleteUser)]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid userId)
        {
            var deleted = await _userService.DeleteUserAsync(userId);

            if (deleted)
                return NoContent();

            return NotFound();
        }
    }
}
