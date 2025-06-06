using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Work_with_db.Tables;

namespace Work_with_db.Repo
{
    public class ClothingItemRepository : GenericRepository<ClothingItem>, IClothingItemRepository
    {
        public ClothingItemRepository(ClothingStoreDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ClothingItem>> GetAllWithDetailsAsync()
        {
            return await set.Include(ci => ci.Size).Include(ci => ci.Brand).ToListAsync();
        }

        public async Task<ClothingItem?> GetByIdWithDetailsAsync(int id)
        {
            return await set.Include(ci => ci.Size).Include(ci => ci.Brand).FirstOrDefaultAsync(ci => ci.Id == id);
        }
    }
}
