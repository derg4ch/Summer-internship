using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Work_with_db.Tables;

namespace Work_with_db.Repo
{
    public class SizeRepository : GenericRepository<Size>, ISizeRepository
    {
        public SizeRepository(ClothingStoreDbContext context) : base(context)
        {

        }

        public async Task<Size?> GetByNameAsync(string name)
        {
            return await set.FirstOrDefaultAsync(s => s.Name == name);
        }

        public async Task<IEnumerable<Size>> GetAllWithClothingItemsAsync()
        {
            return await set.Include(s => s.ClothingItems).ToListAsync();
        }
    }
}
