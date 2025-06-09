using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Work_with_db.Tables;

namespace Work_with_db.Repo
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ClothingStoreDbContext context) : base(context)
        {

        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await set.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await set.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserWithOrdersAsync(int userId)
        {
            return await set.Include(u => u.Orders).FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<IEnumerable<User>> GetAllWithOrdersAsync()
        {
            return await set.Include(u => u.Orders).ToListAsync();
        }

        public async Task<int> GetOrdersCountByUserIdAsync(int userId)
        {
            return await context.Orders.CountAsync(o => o.UserId == userId);
        }

        public async Task<List<User>> GetPagedUsersAsync(int skip, int take)
        {
            return await set.OrderBy(p => p.Id).Skip(skip).Take(take).ToListAsync();
        }
    }
}
