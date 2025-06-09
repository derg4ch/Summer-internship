using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.dto.size;

namespace Logic.Interfaces
{
    public interface ISizeService
    {
        Task<IEnumerable<SizeInfoDto>> GetAllWithClothingItemsCountAsync();
        Task<SizeInfoDto?> GetByIdAsync(int id);
        Task<SizeInfoDto?> GetByNameAsync(string name);
        Task<SizeInfoDto> CreateAsync(SizeNewDto newDto);
        Task<SizeInfoDto?> UpdateAsync(int id, SizeEditDto editDto);
        Task<bool> DeleteAsync(int id);
    }
}
