using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.dto.order;
using Logic.Interfaces;
using Work_with_db.Repo;
using Work_with_db.Tables;

namespace Logic.Services
{
    public class OrderService : IOrderService
    {
        private IOrderRepository repo;

        public OrderService(IOrderRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IEnumerable<OrderInfoDto>> GetAllWithDetailsAsync()
        {
            var orders = await repo.GetAllWithDetailsAsync();

            return orders.Select(newOrder => new OrderInfoDto
            {
                Id = newOrder.Id,
                OrderDate = newOrder.OrderDate,
                Status = newOrder.Status,
                UserId = newOrder.UserId,
                UserEmail = newOrder.User.Email,
                TotalItems = newOrder.OrderItems?.Sum(oi => oi.Quantity) ?? 0,
                TotalPrice = newOrder.OrderItems?.Sum(oi => oi.Quantity * oi.ClothingItem.Price) ?? 0m
            });
        }

        public async Task<OrderInfoDto?> GetByIdWithDetailsAsync(int id)
        {
            Order? order = await repo.GetByIdWithDetailsAsync(id);
            if (order == null) return null;

            return new OrderInfoDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status,
                UserId = order.UserId,
                UserEmail = order.User?.Email ?? "",
                TotalItems = order.OrderItems?.Sum(oi => oi.Quantity) ?? 0,
                TotalPrice = order.OrderItems?.Sum(oi => oi.Quantity * oi.ClothingItem.Price) ?? 0m
            };
        }

        public async Task<IEnumerable<OrderInfoDto>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await repo.GetOrdersByUserIdAsync(userId);

            return orders.Select(order => new OrderInfoDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status,
                UserId = order.UserId,
                UserEmail = order.User.Email,
                TotalItems = order.OrderItems?.Sum(oi => oi.Quantity) ?? 0,
                TotalPrice = order.OrderItems?.Sum(oi => oi.Quantity * oi.ClothingItem.Price) ?? 0m
            });
        }

        public async Task<IEnumerable<OrderInfoDto>> GetOrdersByStatusAsync(string status)
        {
            var orders = await repo.GetOrdersByStatusAsync(status);

            return orders.Select(order => new OrderInfoDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status,
                UserId = order.UserId,
                UserEmail = order.User.Email,
                TotalItems = order.OrderItems?.Sum(oi => oi.Quantity) ?? 0,
                TotalPrice = order.OrderItems?.Sum(oi => oi.Quantity * oi.ClothingItem.Price) ?? 0m
            });
        }

        public async Task<OrderInfoDto> CreateAsync(OrderNewDto newDto)
        {
            Order order = new Order
            {
                UserId = newDto.UserId,
                OrderDate = newDto.OrderDate,
                Status = newDto.Status,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ClothingItemId = newDto.ClothingItemId,
                        Quantity = newDto.Quantity
                    }
                }
            };

            await repo.AddAsync(order);

            var created = await repo.GetByIdWithDetailsAsync(order.Id);

            return new OrderInfoDto
            {
                Id = created!.Id,
                OrderDate = created.OrderDate,
                Status = created.Status,
                UserId = created.UserId,
                UserEmail = created.User.Email,
                TotalItems = created.OrderItems?.Sum(oi => oi.Quantity) ?? 0,
                TotalPrice = created.OrderItems?.Sum(oi => oi.Quantity * oi.ClothingItem.Price) ?? 0m
            };
        }

        public async Task<OrderInfoDto?> UpdateStatusAsync(int id, OrderEditDto editDto)
        {
            Order? order = await repo.GetByIdWithDetailsAsync(id);
            if (order == null) return null;

            order.Status = editDto.Status;
            await repo.UpdateAsync(order);

            Order? updated = await repo.GetByIdWithDetailsAsync(id);

            return new OrderInfoDto
            {
                Id = updated!.Id,
                OrderDate = updated.OrderDate,
                Status = updated.Status,
                UserId = updated.UserId,
                UserEmail = updated.User.Email,
                TotalItems = updated.OrderItems?.Sum(oi => oi.Quantity) ?? 0,
                TotalPrice = updated.OrderItems?.Sum(oi => oi.Quantity * oi.ClothingItem.Price) ?? 0m
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await repo.GetByIdAsync(id);
            if (order == null) return false;

            await repo.DeleteAsync(order);
            return true;
        }
    }
}
