using A_Domain.Models;
using A_Domain.Repo_interfaces;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DataAccess.Repositories
{
    public class StockRepository : GenericRepository<Stock, Guid>, IStockRepository
    {
        public StockRepository(EcommerceIdentityDbContext context) : base(context)
        {
        }
    }
}
