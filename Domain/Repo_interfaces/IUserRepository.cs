using A_Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace A_Domain.Repo_interfaces
{
    public interface IUserRepository : IGenericRepository<UserIdentity, Guid>
    {
    }
}
