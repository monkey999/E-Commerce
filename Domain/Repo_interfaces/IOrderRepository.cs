using A_Domain.Models;
using System;

namespace A_Domain.Repo_interfaces
{
    public interface IOrderRepository : IGenericRepository<Order, Guid>
    {

    }
}
