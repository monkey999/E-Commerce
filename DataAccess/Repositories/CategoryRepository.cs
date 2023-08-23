using A_Domain.Models;
using A_Domain.Repo_interfaces;
using B_DataAccess;
using System;

namespace DataAccess.Repositories
{
    public class CategoryRepository : GenericRepository<Category, Guid>, ICategoryRepository
    {
        public CategoryRepository(EcommerceIdentityDbContext context) : base(context)
        {

        }
    }
}
