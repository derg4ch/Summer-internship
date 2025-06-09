using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.dto.order_item;
using Logic.Interfaces;
using Work_with_db.Repo;
using Work_with_db.Tables;

namespace Logic.Services
{
    public class OrderItemService : IOrderItemService
    {
        private IOrderItemRepository repo;
        private readonly IClothingItemRepository clothingItemRepo;

        public OrderItemService(IOrderItemRepository repo, IClothingItemRepository clothingItemRepo)
        {
            this.repo = repo;
            this.clothingItemRepo = clothingItemRepo;
        }


        public async Task<IEnumerable<OrderItemInfoDto>> GetAllWithDetailsAsync()
        {
            var items = await repo.GetAllWithDetailsAsync();

            return items.Select(orderItem => new OrderItemInfoDto
            {
                Id = orderItem.Id,
                OrderId = orderItem.OrderId,
                OrderDate = orderItem.Order.OrderDate,
                OrderStatus = orderItem.Order.Status,
                ClothingItemId = orderItem.ClothingItemId,
                ClothingItemName = orderItem.ClothingItem.Name,
                UnitPrice = orderItem.ClothingItem.Price,
                Quantity = orderItem.Quantity
            });
        }

        public async Task<OrderItemInfoDto?> GetByIdWithDetailsAsync(int id)
        {
            OrderItem? orderItem = await repo.GetByIdWithDetailsAsync(id);
            if (orderItem == null)
            {
                return null;
            }

            return new OrderItemInfoDto
            {
                Id = orderItem.Id,
                OrderId = orderItem.OrderId,
                OrderDate = orderItem.Order.OrderDate,
                OrderStatus = orderItem.Order.Status,
                ClothingItemId = orderItem.ClothingItemId,
                ClothingItemName = orderItem.ClothingItem.Name,
                UnitPrice = orderItem.ClothingItem.Price,
                Quantity = orderItem.Quantity
            };
        }

        public async Task<IEnumerable<OrderItemInfoDto>> GetByOrderIdAsync(int orderId)
        {
            var items = await repo.GetByOrderIdAsync(orderId);

            return items.Select(orderItem => new OrderItemInfoDto
            {
                Id = orderItem.Id,
                OrderId = orderItem.OrderId,
                OrderDate = orderItem.Order.OrderDate,
                OrderStatus = orderItem.Order.Status,
                ClothingItemId = orderItem.ClothingItemId,
                ClothingItemName = orderItem.ClothingItem.Name,
                UnitPrice = orderItem.ClothingItem.Price,
                Quantity = orderItem.Quantity
            });
        }

        public async Task<OrderItemInfoDto> CreateAsync(OrderItemNewDto newDto)
        {
            OrderItem orderItem = new OrderItem
            {
                OrderId = newDto.OrderId,
                ClothingItemId = newDto.ClothingItemId,
                Quantity = newDto.Quantity
            };

            ClothingItem? clothingItem = await clothingItemRepo.GetByIdAsync(newDto.ClothingItemId);
            if (clothingItem == null)
            {
                throw new Exception("Clothing item not found.");
            }

            if (clothingItem.Quantity < newDto.Quantity)
            {
                throw new Exception("Not enough quantity in stock.");
            }

            clothingItem.Quantity -= newDto.Quantity;
            await clothingItemRepo.UpdateAsync(clothingItem);

            await repo.AddAsync(orderItem);

            OrderItem? created = await repo.GetByIdWithDetailsAsync(orderItem.Id);

            return new OrderItemInfoDto
            {
                Id = created!.Id,
                OrderId = created.OrderId,
                OrderDate = created.Order.OrderDate,
                OrderStatus = created.Order.Status,
                ClothingItemId = created.ClothingItemId,
                ClothingItemName = created.ClothingItem.Name,
                UnitPrice = created.ClothingItem.Price,
                Quantity = created.Quantity
            };
        }

        public async Task<OrderItemInfoDto?> UpdateAsync(int id, OrderItemEditDto editDto)
        {
            OrderItem? orderItem = await repo.GetByIdWithDetailsAsync(id);
            if (orderItem == null)
            {
                return null;
            }

            ClothingItem? clothingItem = await clothingItemRepo.GetByIdAsync(orderItem.ClothingItemId);
            if (clothingItem == null)
            {
                throw new Exception("Clothing item not found.");
            }

            int difference = editDto.Quantity - orderItem.Quantity;

            if (difference > 0)
            {
                if (clothingItem.Quantity < difference)
                {
                    throw new Exception("Not enough quantity in stock.");
                }
                clothingItem.Quantity -= difference;
            }
            else if (difference < 0)
            {
                clothingItem.Quantity += -difference;
            }

            orderItem.Quantity = editDto.Quantity;
            await repo.UpdateAsync(orderItem);
            await clothingItemRepo.UpdateAsync(clothingItem);

            var updated = await repo.GetByIdWithDetailsAsync(id);

            return new OrderItemInfoDto
            {
                Id = updated!.Id,
                OrderId = updated.OrderId,
                OrderDate = updated.Order.OrderDate,
                OrderStatus = updated.Order.Status,
                ClothingItemId = updated.ClothingItemId,
                ClothingItemName = updated.ClothingItem.Name,
                UnitPrice = updated.ClothingItem.Price,
                Quantity = updated.Quantity
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            OrderItem? orderItem = await repo.GetByIdAsync(id);
            if (orderItem == null)
            {
                return false;
            }

            await repo.DeleteAsync(orderItem);
            return true;
        }
    }
}
