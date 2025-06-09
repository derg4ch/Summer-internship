using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Work_with_db.Tables;

namespace Work_with_db.Repo
{
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(ClothingStoreDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<OrderItem>> GetAllWithDetailsAsync()
        {
            return await set.Include(oi => oi.Order).Include(oi => oi.ClothingItem).ToListAsync();
        }

        public async Task<OrderItem?> GetByIdWithDetailsAsync(int id)
        {
            return await set.Include(oi => oi.Order).Include(oi => oi.ClothingItem).FirstOrDefaultAsync(oi => oi.Id == id);
        }

        public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId)
        {
            return await set.Include(oi => oi.ClothingItem).Include(oi => oi.Order).Where(oi => oi.OrderId == orderId).ToListAsync();
        }
    }
}
