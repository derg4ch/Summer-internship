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
            var allOrders = await repo.GetAllWithDetailsAsync();
         
            return allOrders.Select(p => new OrderInfoDto
            {
                Id = p.Id,
                OrderDate = p.OrderDate,
                Status = p.Status,
                UserId = p.UserId,
                UserEmail = p.User.Email,
                TotalItems = p.OrderItems?.Sum(orderItems => orderItems.Quantity) ?? 0,
                TotalPrice = p.OrderItems?.Sum(orderItems => orderItems.Quantity * orderItems.ClothingItem.Price) ?? 0m
            });
        }

        public async Task<OrderInfoDto?> GetByIdWithDetailsAsync(int id)
        {
            Order? orderById = await repo.GetByIdWithDetailsAsync(id);
            if (orderById == null)
            {
                return null;
            }
            OrderInfoDto orderInfoDto = new OrderInfoDto
            {
                Id = orderById.Id,
                OrderDate = orderById.OrderDate,
                Status = orderById.Status,
                UserId = orderById.UserId,
                UserEmail = orderById.User.Email,
                TotalItems = orderById.OrderItems?.Sum(orderItems => orderItems.Quantity) ?? 0,
                TotalPrice = orderById.OrderItems?.Sum(orderItems => orderItems.Quantity * orderItems.ClothingItem.Price) ?? 0m
            };

            return orderInfoDto;
        }

        public async Task<IEnumerable<OrderInfoDto>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await repo.GetOrdersByUserIdAsync(userId);

            return orders.Select(p => new OrderInfoDto
            {
                Id = p.Id,
                OrderDate = p.OrderDate,
                Status = p.Status,
                UserId = p.UserId,
                UserEmail = p.User.Email,
                TotalItems = p.OrderItems?.Sum(orderItems => orderItems.Quantity) ?? 0,
                TotalPrice = p.OrderItems?.Sum(orderItems => orderItems.Quantity * orderItems.ClothingItem.Price) ?? 0m
            });
        }

        public async Task<IEnumerable<OrderInfoDto>> GetOrdersByStatusAsync(string status)
        {
            var ordersByStatus = await repo.GetOrdersByStatusAsync(status);

            return ordersByStatus.Select(p => new OrderInfoDto
            {
                Id = p.Id,
                OrderDate = p.OrderDate,
                Status = p.Status,
                UserId = p.UserId,
                UserEmail = p.User.Email,
                TotalItems = p.OrderItems?.Sum(orderItems => orderItems.Quantity) ?? 0,
                TotalPrice = p.OrderItems?.Sum(orderItems => orderItems.Quantity * orderItems.ClothingItem.Price) ?? 0m
            });
        }

        public async Task<OrderInfoDto> CreateAsync(OrderNewDto newDto)
        {
            Order order = new Order
            {
                UserId = newDto.UserId,
                OrderDate = newDto.OrderDate,
                Status = "Pending",
            };

            await repo.AddAsync(order); 

            OrderItem orderItem = new OrderItem
            {
                OrderId = order.Id,
                ClothingItemId = newDto.ClothingItemId,
                Quantity = newDto.Quantity
            };

            await repo.AddOrderItemAsync(orderItem);
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
            Order? orderById = await repo.GetByIdWithDetailsAsync(id);
            if (orderById == null)
            {
                return null;
            }

            orderById.Status = editDto.Status;
            await repo.UpdateAsync(orderById);

            Order? updated = await repo.GetByIdWithDetailsAsync(id);
            
            OrderInfoDto orderInfoDto = new OrderInfoDto
            {
                Id = updated!.Id,
                OrderDate = updated.OrderDate,
                Status = updated.Status,
                UserId = updated.UserId,
                UserEmail = updated.User.Email,
                TotalItems = updated.OrderItems?.Sum(orderItems => orderItems.Quantity) ?? 0,
                TotalPrice = updated.OrderItems?.Sum(orderItems => orderItems.Quantity * orderItems.ClothingItem.Price) ?? 0m
            };
            return orderInfoDto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var orderById = await repo.GetByIdAsync(id);
            
            if (orderById == null)
            {
                return false;
            }

            await repo.DeleteAsync(orderById);
            return true;
        }

        public async Task<PagedList<OrderInfoDto>> GetPagedOrdersAsync(int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;
            var orders = await repo.GetPagedWithDetailsAsync(skip, pageSize);
            int totalCount = await repo.getAllCount();

            var orderDtos = orders.Select(p => new OrderInfoDto
            {
                Id = p.Id,
                OrderDate = p.OrderDate,
                Status = p.Status,
                UserId = p.UserId,
                UserEmail = p.User.Email,
                TotalItems = p.OrderItems?.Sum(orderItem => orderItem.Quantity) ?? 0,
                TotalPrice = p.OrderItems?.Sum(orderItem => orderItem.Quantity * orderItem.ClothingItem.Price) ?? 0m
            }).ToList();

            return new PagedList<OrderInfoDto>(orderDtos, pageNumber, pageSize, totalCount);
        }
    }
}
