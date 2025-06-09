using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.dto.clothing_item;

namespace Logic.Interfaces
{
    public interface IClothingItemService
    {
        Task<IEnumerable<ClothingItemInfoDto>> GetAllWithDetailsAsync();
        Task<ClothingItemInfoDto?> GetByIdWithDetailsAsync(int id);
        Task<ClothingItemInfoDto> CreateAsync(ClothingItemNewDto newDto);
        Task<ClothingItemInfoDto?> UpdateAsync(int id, ClothingItemEditDto editDto);
        Task<bool> DeleteAsync(int id);
        Task<PagedList<ClothingItemInfoDto>> GetPagedClothingItemsAsync(int pageNumber, int pageSize);
    }
}
