using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.dto.order_item;

namespace Logic.Interfaces
{
    public interface IOrderItemService
    {
        Task<int> GetOrderUserIdAsync(int orderId);
        Task<IEnumerable<OrderItemInfoDto>> GetAllWithDetailsAsync();
        Task<OrderItemInfoDto?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<OrderItemInfoDto>> GetByOrderIdAsync(int orderId);
        Task<OrderItemInfoDto> CreateAsync(OrderItemNewDto newDto);
        Task<OrderItemInfoDto?> UpdateAsync(int id, OrderItemEditDto editDto);
        Task<bool> DeleteAsync(int id);
    }
}
