using System.Threading.Tasks;

namespace A_Domain.Repo_interfaces
{
    public interface IUnitOfWork
    {
        ICategoryRepository Categories { get; }
        IOrderRepository Orders { get; }
        IProductRepository Products { get; }
        IUserRepository Users { get; }

        void Dispose();
        Task SaveAsync();
    }
}