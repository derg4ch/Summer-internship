using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.dto.size;
using Logic.Interfaces;
using Work_with_db.Repo;
using Work_with_db.Tables;

namespace Logic.Services
{
    public class SizeService : ISizeService
    {
        private ISizeRepository repo;

        public SizeService(ISizeRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IEnumerable<SizeInfoDto>> GetAllWithClothingItemsCountAsync()
        {
            var allSize = await repo.GetAllWithClothingItemsAsync();

            return allSize.Select(s => new SizeInfoDto
            {
                Id = s.Id,
                Name = s.Name,
                ClothingItemsCount = s.ClothingItems?.Count ?? 0
            });
        }

        public async Task<SizeInfoDto?> GetByIdAsync(int id)
        {
            Size? size = await repo.GetByIdAsync(id);
            if (size == null)
            {
                return null;
            }

            int count = await repo.GetClothingItemsCountBySizeIdAsync(id);
            SizeInfoDto sizeInfoDto = new SizeInfoDto
            {
                Id = size.Id,
                Name = size.Name,
                ClothingItemsCount = count
            };

            return sizeInfoDto;
        }

        public async Task<SizeInfoDto?> GetByNameAsync(string name)
        {
            Size? size = await repo.GetByNameAsync(name);
            if (size == null)
            {
                return null;
            }
            int count = await repo.GetClothingItemsCountBySizeIdAsync(size.Id);

            SizeInfoDto sizeInfoDto = new SizeInfoDto
            {
                Id = size.Id,
                Name = size.Name,
                ClothingItemsCount = count
            };

            return sizeInfoDto;
        }

        public async Task<SizeInfoDto> CreateAsync(SizeNewDto newDto)
        {
            Size size = new Size
            {
                Name = newDto.Name
            };

            await repo.AddAsync(size);
            
            SizeInfoDto sizeInfoDto = new SizeInfoDto
            {
                Id = size.Id,
                Name = size.Name,
                ClothingItemsCount = 0
            };
            return sizeInfoDto;
        }

        public async Task<SizeInfoDto?> UpdateAsync(int id, SizeEditDto editDto)
        {
            Size? size = await repo.GetByIdAsync(id);
            if (size == null)
            {
                return null;
            }

            size.Name = editDto.Name;
            await repo.UpdateAsync(size);
            
            int count = await repo.GetClothingItemsCountBySizeIdAsync(size.Id);
            
            SizeInfoDto sizeInfoDto = new SizeInfoDto
            {
                Id = size.Id,
                Name = size.Name,
                ClothingItemsCount = count
            };
            return sizeInfoDto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Size? size = await repo.GetByIdAsync(id);
            if (size == null)
            {
                return false;
            }

            await repo.DeleteAsync(size);
            return true;
        }
    }
}
