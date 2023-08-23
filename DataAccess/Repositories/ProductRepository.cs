using A_Domain.Models;
using A_Domain.Repo_interfaces;
using B_DataAccess;
using System;

namespace DataAccess.Repositories
{
    public class ProductRepository : GenericRepository<Product, Guid>, IProductRepository
    {
        public ProductRepository(EcommerceIdentityDbContext context) : base(context)
        {
        }
    }
}
