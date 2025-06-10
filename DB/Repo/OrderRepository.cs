using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Work_with_db.Tables;

namespace Work_with_db.Repo
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ClothingStoreDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Order>> GetAllWithDetailsAsync()
        {
            return await set.Include(o => o.User).Include(o => o.OrderItems).ThenInclude(oi => oi.ClothingItem).ToListAsync();
        }

        public async Task<Order?> GetByIdWithDetailsAsync(int id)
        {
            return await set.Include(o => o.User).Include(o => o.OrderItems).ThenInclude(oi => oi.ClothingItem).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await set.Include(o => o.User).Include(o => o.OrderItems).ThenInclude(oi => oi.ClothingItem).Where(o => o.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status)
        {
            return await set.Include(o => o.User).Include(o => o.OrderItems).ThenInclude(oi => oi.ClothingItem).Where(o => o.Status.ToLower() == status.ToLower()).ToListAsync();
        }
        public async Task<List<Order>> GetPagedWithDetailsAsync(int skip, int take)
        {
            return await set.Include(o => o.User).Include(o => o.OrderItems).ThenInclude(oi => oi.ClothingItem).OrderBy(o => o.Id).Skip(skip).Take(take).ToListAsync();
        }

        public async Task<int> getAllCount()
        {
            return await set.CountAsync();
        }
    }
}
