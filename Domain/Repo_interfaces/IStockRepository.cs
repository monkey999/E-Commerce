using A_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_Domain.Repo_interfaces
{
    public interface IStockRepository : IGenericRepository<Stock, Guid>
    {

    }
}
