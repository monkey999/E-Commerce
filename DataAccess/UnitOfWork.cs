using A_Domain.Repo_interfaces;
using B_DataAccess;
using DataAccess.Repositories;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EcommerceIdentityDbContext _context;
        public ICategoryRepository Categories { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public IProductRepository Products { get; private set; }
        public IUserRepository Users { get; private set; }
            
        public UnitOfWork(EcommerceIdentityDbContext context)
        {
            _context = context;
            Categories = new CategoryRepository(context);
            Orders = new OrderRepository(context);
            Products = new ProductRepository(context);
            Users = new UserRepository(context);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        //public virtual void Dispose(bool disposing)
        //{
        //    if (!disposed)
        //    {
        //        if (disposing)
        //        {
        //            context.Dispose();
        //        }
        //    }
        //    disposed = true;
        //}

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}
    }
}
