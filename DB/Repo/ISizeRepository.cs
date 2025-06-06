using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work_with_db.Tables;

namespace Work_with_db.Repo
{
    public interface ISizeRepository : IGenericRepository<Size>
    {
        Task<Size?> GetByNameAsync(string name);
        Task<IEnumerable<Size>> GetAllWithClothingItemsAsync();
    }
}
