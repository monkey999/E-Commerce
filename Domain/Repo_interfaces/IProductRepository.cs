using A_Domain.Models;
using System;

namespace A_Domain.Repo_interfaces
{
    public interface IProductRepository : IGenericRepository<Product, Guid>
    {
    }
}
