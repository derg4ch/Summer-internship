using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.dto.clothing_item;
using Logic.Interfaces;
using Work_with_db.Repo;
using Work_with_db.Tables;

namespace Logic.Services
{
    public class ClothingItemService : IClothingItemService
    {
        private IClothingItemRepository repo;

        public ClothingItemService(IClothingItemRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IEnumerable<ClothingItemInfoDto>> GetAllWithDetailsAsync()
        {
            var items = await repo.GetAllWithDetailsAsync();

            return items.Select(clothingItem => new ClothingItemInfoDto
            {
                Id = clothingItem.Id,
                Name = clothingItem.Name,
                SizeId = clothingItem.SizeId,
                SizeName = clothingItem.Size.Name,
                BrandId = clothingItem.BrandId,
                BrandName = clothingItem.Brand.Name,
                Price = clothingItem.Price,
                Quantity = clothingItem.Quantity
            });
        }

        public async Task<ClothingItemInfoDto?> GetByIdWithDetailsAsync(int id)
        {
            ClothingItem? clothingItem = await repo.GetByIdWithDetailsAsync(id);
            if (clothingItem == null)
            {
                return null;
            }

            return new ClothingItemInfoDto
            {
                Id = clothingItem.Id,
                Name = clothingItem.Name,
                SizeId = clothingItem.SizeId,
                SizeName = clothingItem.Size.Name ,
                BrandId = clothingItem.BrandId,
                BrandName = clothingItem.Brand.Name,
                Price = clothingItem.Price,
                Quantity = clothingItem.Quantity
            };
        }

        public async Task<ClothingItemInfoDto> CreateAsync(ClothingItemNewDto newDto)
        {
            ClothingItem entity = new ClothingItem
            {
                Name = newDto.Name,
                SizeId = newDto.SizeId,
                BrandId = newDto.BrandId,
                Price = newDto.Price,
                Quantity = newDto.Quantity
            };

            await repo.AddAsync(entity);

            ClothingItem? created = await repo.GetByIdWithDetailsAsync(entity.Id);

            return new ClothingItemInfoDto
            {
                Id = created!.Id,
                Name = created.Name,
                SizeId = created.SizeId,
                SizeName = created.Size.Name,
                BrandId = created.BrandId,
                BrandName = created.Brand.Name,
                Price = created.Price,
                Quantity = created.Quantity
            };
        }

        public async Task<ClothingItemInfoDto?> UpdateAsync(int id, ClothingItemEditDto editDto)
        {
            ClothingItem? entity = await repo.GetByIdWithDetailsAsync(id);
            if (entity == null)
            {
                return null;
            }

            entity.Name = editDto.Name;
            entity.SizeId = editDto.SizeId;
            entity.BrandId = editDto.BrandId;
            entity.Price = editDto.Price;
            entity.Quantity = editDto.Quantity;

            await repo.UpdateAsync(entity);

            ClothingItem? updated = await repo.GetByIdWithDetailsAsync(id);

            return new ClothingItemInfoDto
            {
                Id = updated!.Id,
                Name = updated.Name,
                SizeId = updated.SizeId,
                SizeName = updated.Size.Name,
                BrandId = updated.BrandId,
                BrandName = updated.Brand.Name ,
                Price = updated.Price,
                Quantity = updated.Quantity
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            ClothingItem? entity = await repo.GetByIdAsync(id);
            
            if (entity == null)
            {
                return false;
            }
            await repo.DeleteAsync(entity);
            return true;
        }

        public async Task<PagedList<ClothingItemInfoDto>> GetPagedClothingItemsAsync(int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;
            var items = await repo.GetPagedWithDetailsAsync(skip, pageSize);

            var itemDtos = items.Select(item => new ClothingItemInfoDto
            {
                Id = item.Id,
                Name = item.Name,
                SizeId = item.SizeId,
                SizeName = item.Size.Name,
                BrandId = item.BrandId,
                BrandName = item.Brand.Name,
                Price = item.Price,
                Quantity = item.Quantity
            }).ToList();

            return new PagedList<ClothingItemInfoDto>(itemDtos, pageNumber, pageSize);
        }
    }
}
