using A_Domain.Models;
using A_Domain.Repo_interfaces;
using B_DataAccess;
using Microsoft.AspNetCore.Identity;
using System;

namespace DataAccess.Repositories
{
    public class UserRepository : GenericRepository<UserIdentity, Guid>, IUserRepository
    {
        public UserRepository(EcommerceIdentityDbContext context) : base(context)
        {

        }
    }
}
