using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.dto.brand;

namespace Logic.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandInfoDto>> GetAllAsync();
        Task<BrandInfoDto?> GetByIdAsync(int id);
        Task<IEnumerable<BrandInfoDto>> GetAllWithClothingItemsAsync();
        Task<BrandInfoDto?> GetByIdWithClothingItemsAsync(int id);
        Task<BrandInfoDto> CreateAsync(BrandNewDto newBrand);
        Task<bool> UpdateAsync(int id, BrandEditDto updatedBrand);
        Task<bool> DeleteAsync(int id);
    }
}
