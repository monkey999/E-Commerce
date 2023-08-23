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
    public class StockOnHoldRepository : GenericRepository<StockOnHold, Guid>, IStockOnHoldRepository
    {
        public StockOnHoldRepository(EcommerceIdentityDbContext context) : base(context)
        {
        }
    }
}
