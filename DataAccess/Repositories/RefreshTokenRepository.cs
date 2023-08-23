using A_Domain.Models;
using A_Domain.Repo_interfaces;
using B_DataAccess;

namespace DataAccess.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken, string>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(EcommerceIdentityDbContext context) : base(context)
        {

        }
    }
}