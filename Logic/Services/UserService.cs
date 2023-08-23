using A_Domain.Models;
using A_Domain.Repo_interfaces;
using E_Commerce_Shop.Contracts.V1.DTO_requests.CREATE;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserIdentity> _userManager;
        private readonly IGenericRepository<UserIdentity, Guid> _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<UserIdentity> userManager, IGenericRepository<UserIdentity, Guid> userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IdentityResult> CreateUserAsync(CreateUserRequestDTO request)
        {
            var newUser = new UserIdentity()
            {
                Id = request.UserId.ToString(),
                UserName = request.UserName,
                Name = request.Name,
                Surname = request.Surname,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Address = request.Adress
            };

            var created = await _userManager.CreateAsync(newUser, request.Password);

            return created;
        }

        public async Task<IEnumerable<UserIdentity>> GetUsersAsync()
        {
            var users = await _userRepository.GetAll()
                            .OrderBy(u => u.Id)
                                .ToListAsync();

            return users;
        }

        public async Task<UserIdentity> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.FindByCondition(u => u.Id.Equals(userId))
                            .SingleOrDefaultAsync();

            return user;
        }

        public async Task<UserIdentity> GetUserAsync()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            return user;
        }

        public async Task<IdentityResult> UpdateUserAsync(UserIdentity user)
        {
            var updated = await _userManager.UpdateAsync(user);

            return updated;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await GetUserByIdAsync(userId);

            if (user == null)
                return false;

            var deleted = await _userManager.DeleteAsync(user);

            return deleted.Succeeded;
        }
    }
}
