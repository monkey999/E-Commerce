using A_Domain.Models;
using System;
using System.Linq;

namespace A_Domain.Repo_interfaces
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken, string>
    {
    }
}
