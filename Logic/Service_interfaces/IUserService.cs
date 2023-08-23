using A_Domain.Models;
using E_Commerce_Shop.Contracts.V1.DTO_requests.CREATE;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Services
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUserAsync(CreateUserRequestDTO request);
        Task<IEnumerable<UserIdentity>> GetUsersAsync();
        Task<UserIdentity> GetUserByIdAsync(Guid userId);
        Task<UserIdentity> GetUserAsync();
        Task<IdentityResult> UpdateUserAsync(UserIdentity user);
        Task<bool> DeleteUserAsync(Guid userId);
    }
}