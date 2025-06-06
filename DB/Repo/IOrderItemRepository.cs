using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work_with_db.Tables;

namespace Work_with_db.Repo
{
    public interface IOrderItemRepository : IGenericRepository<OrderItem>
    {
        Task<IEnumerable<OrderItem>> GetAllWithDetailsAsync();
        Task<OrderItem?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId);
    }
}
