using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work_with_db.Tables;

namespace Work_with_db.Repo
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetUserWithOrdersAsync(int userId);
        Task<IEnumerable<User>> GetAllWithOrdersAsync();
        Task<int> GetOrdersCountByUserIdAsync(int userId);
    }
}
