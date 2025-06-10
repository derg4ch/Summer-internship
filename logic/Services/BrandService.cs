using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.dto.brand;
using Logic.Interfaces;
using Work_with_db.Repo;
using Work_with_db.Tables;

namespace Logic.Services
{
    public class BrandService : IBrandService
    {
        private IBrandRepository repository;

        public BrandService(IBrandRepository brandRepository)
        {
            repository = brandRepository;
        }

        public async Task<IEnumerable<BrandInfoDto>> GetAllAsync()
        {
            var allBrands = await repository.GetAllAsync();
            return allBrands.Select(brand => new BrandInfoDto
            {
                Id = brand.Id,
                Name = brand.Name,
                Country = brand.Country
            });
        }

        public async Task<BrandInfoDto?> GetByIdAsync(int id)
        {
            Brand? brand = await repository.GetByIdAsync(id);
            if (brand == null)
            {
                return null;
            }

            BrandInfoDto brandInfoDto = new BrandInfoDto
            {
                Id = brand.Id,
                Name = brand.Name,
                Country = brand.Country
            };

            return brandInfoDto;
        }

        public async Task<BrandInfoDto> CreateAsync(BrandNewDto newBrand)
        {
            Brand brand = new Brand
            {
                Name = newBrand.Name,
                Country = newBrand.Country
            };

            await repository.AddAsync(brand);
            BrandInfoDto brandInfoDto = new BrandInfoDto
            {
                Id = brand.Id,
                Name = brand.Name,
                Country = brand.Country
            };

            return brandInfoDto;
        }

        public async Task<bool> UpdateAsync(int id, BrandEditDto updatedBrand)
        {
            Brand? existingBrand = await repository.GetByIdAsync(id);
            if (existingBrand == null)
            {
                return false;
            }

            existingBrand.Name = updatedBrand.Name;
            existingBrand.Country = updatedBrand.Country;

            await repository.UpdateAsync(existingBrand);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Brand? brand = await repository.GetByIdAsync(id);
            if (brand == null)
            {
                return false;
            }

            await repository.DeleteAsync(brand);
            return true;
        }
    }
}
