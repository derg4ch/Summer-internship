using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work_with_db.Tables;

namespace Work_with_db.Repo
{
    public interface IClothingItemRepository : IGenericRepository<ClothingItem>
    {
        Task<IEnumerable<ClothingItem>> GetAllWithDetailsAsync();
        Task<ClothingItem?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<ClothingItem>> GetPagedWithDetailsAsync(int skip, int take);
    }
}
