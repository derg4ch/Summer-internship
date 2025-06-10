using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work_with_db.Tables;

namespace Work_with_db.Repo
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<int> getAllCount();
        Task<IEnumerable<Order>> GetAllWithDetailsAsync();
        Task<Order?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status);
        Task<List<Order>> GetPagedWithDetailsAsync(int skip, int take);
    }
}
