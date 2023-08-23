using A_Domain.Models;
using A_Domain.Repo_interfaces;
using B_DataAccess;
using System;

namespace DataAccess.Repositories
{
    public class OrderRepository : GenericRepository<Order, Guid>, IOrderRepository
    {
        public OrderRepository(EcommerceIdentityDbContext context) : base(context)
        {
        }
    }
}
