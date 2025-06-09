using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.dto.order;

namespace Logic.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderInfoDto>> GetAllWithDetailsAsync();
        Task<OrderInfoDto?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<OrderInfoDto>> GetOrdersByUserIdAsync(int userId);
        Task<IEnumerable<OrderInfoDto>> GetOrdersByStatusAsync(string status);
        Task<OrderInfoDto> CreateAsync(OrderNewDto newDto);
        Task<OrderInfoDto?> UpdateStatusAsync(int id, OrderEditDto editDto);
        Task<bool> DeleteAsync(int id);
    }
}
