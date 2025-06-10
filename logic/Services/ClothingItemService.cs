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
            var allClothingItems = await repo.GetAllWithDetailsAsync();

            return allClothingItems.Select(p => new ClothingItemInfoDto
            {
                Id = p.Id,
                Name = p.Name,
                SizeId = p.SizeId,
                SizeName = p.Size.Name,
                BrandId = p.BrandId,
                BrandName = p.Brand.Name,
                Price = p.Price,
                Quantity = p.Quantity
            });
        }

        public async Task<ClothingItemInfoDto?> GetByIdWithDetailsAsync(int id)
        {
            ClothingItem? clothingItem = await repo.GetByIdWithDetailsAsync(id);
            if (clothingItem == null)
            {
                return null;
            }
            ClothingItemInfoDto clothingItemInfoDto = new ClothingItemInfoDto
            {
                Id = clothingItem.Id,
                Name = clothingItem.Name,
                SizeId = clothingItem.SizeId,
                SizeName = clothingItem.Size.Name,
                BrandId = clothingItem.BrandId,
                BrandName = clothingItem.Brand.Name,
                Price = clothingItem.Price,
                Quantity = clothingItem.Quantity
            };

            return clothingItemInfoDto;
        }

        public async Task<ClothingItemInfoDto> CreateAsync(ClothingItemNewDto newDto)
        {
            ClothingItem newClothingItem = new ClothingItem
            {
                Name = newDto.Name,
                SizeId = newDto.SizeId,
                BrandId = newDto.BrandId,
                Price = newDto.Price,
                Quantity = newDto.Quantity
            };

            await repo.AddAsync(newClothingItem);

            ClothingItem? created = await repo.GetByIdWithDetailsAsync(newClothingItem.Id);
            ClothingItemInfoDto clothingItemInfoDto = new ClothingItemInfoDto
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

            return clothingItemInfoDto;
        }

        public async Task<ClothingItemInfoDto?> UpdateAsync(int id, ClothingItemEditDto editDto)
        {
            ClothingItem? clothingItemById = await repo.GetByIdWithDetailsAsync(id);
            if (clothingItemById == null)
            {
                return null;
            }

            clothingItemById.Name = editDto.Name;
            clothingItemById.SizeId = editDto.SizeId;
            clothingItemById.BrandId = editDto.BrandId;
            clothingItemById.Price = editDto.Price;
            clothingItemById.Quantity = editDto.Quantity;

            await repo.UpdateAsync(clothingItemById);

            ClothingItem? updated = await repo.GetByIdWithDetailsAsync(id);
            ClothingItemInfoDto clothingItemInfoDto = new ClothingItemInfoDto
            {
                Id = updated!.Id,
                Name = updated.Name,
                SizeId = updated.SizeId,
                SizeName = updated.Size.Name,
                BrandId = updated.BrandId,
                BrandName = updated.Brand.Name,
                Price = updated.Price,
                Quantity = updated.Quantity
            };

            return clothingItemInfoDto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            ClothingItem? clothingItemById = await repo.GetByIdAsync(id);
            
            if (clothingItemById == null)
            {
                return false;
            }
            await repo.DeleteAsync(clothingItemById);
            return true;
        }

        public async Task<PagedList<ClothingItemInfoDto>> GetPagedClothingItemsAsync(int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;
            var items = await repo.GetPagedWithDetailsAsync(skip, pageSize);
            int totalCount = await repo.getAllCount();

            var itemDtos = items.Select(p => new ClothingItemInfoDto
            {
                Id = p.Id,
                Name = p.Name,
                SizeId = p.SizeId,
                SizeName = p.Size.Name,
                BrandId = p.BrandId,
                BrandName = p.Brand.Name,
                Price = p.Price,
                Quantity = p.Quantity
            }).ToList();

            return new PagedList<ClothingItemInfoDto>(itemDtos, pageNumber, pageSize, totalCount);
        }

        public async Task<PagedList<ClothingItemInfoDto>> GetFilteredClothingItemsAsync(ClothingItemFilterDto filter)
        {
            var allItems = await repo.GetAllWithDetailsAsync();
            var queryable = allItems.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                queryable = queryable.Where(x => x.Name.Contains(filter.Name));
            }
            if (filter.BrandId.HasValue)
            {
                queryable = queryable.Where(x => x.BrandId == filter.BrandId.Value);
            }
            if (!string.IsNullOrWhiteSpace(filter.BrandName))
            {
                queryable = queryable.Where(x => x.Brand.Name.Contains(filter.BrandName));
            }
            if (!string.IsNullOrWhiteSpace(filter.BrandCountry))
            {
                queryable = queryable.Where(x => x.Brand.Country.Contains(filter.BrandCountry));
            }
            if (filter.SizeId.HasValue)
            {
                queryable = queryable.Where(x => x.SizeId == filter.SizeId.Value);
            }
            if (!string.IsNullOrWhiteSpace(filter.SizeName))
            {
                queryable = queryable.Where(x => x.Size.Name.Contains(filter.SizeName));
            }
            if (filter.MinPrice.HasValue)
            {
                queryable = queryable.Where(x => x.Price >= filter.MinPrice.Value);
            }
            if (filter.MaxPrice.HasValue)
            {
                queryable = queryable.Where(x => x.Price <= filter.MaxPrice.Value);
            }
            if (filter.MinQuantity.HasValue)
            {
                queryable = queryable.Where(x => x.Quantity >= filter.MinQuantity.Value);
            }
            if (filter.MaxQuantity.HasValue)
            {
                queryable = queryable.Where(x => x.Quantity <= filter.MaxQuantity.Value);
            }

            int skip = (filter.PageNumber - 1) * filter.PageSize;
            var paginatedFiltered = queryable.Skip(skip).Take(filter.PageSize);
            int totalCount = queryable.Count();

            var result = paginatedFiltered.Select(p => new ClothingItemInfoDto
            {
                Id = p.Id,
                Name = p.Name,
                SizeId = p.SizeId,
                SizeName = p.Size.Name,
                BrandId = p.BrandId,
                BrandName = p.Brand.Name,
                Price = p.Price,
                Quantity = p.Quantity
            }).ToList();

            return new PagedList<ClothingItemInfoDto>(result, filter.PageNumber, filter.PageSize, totalCount);
        }
    }
}
