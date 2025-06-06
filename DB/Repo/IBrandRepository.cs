using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work_with_db.Tables;

namespace Work_with_db.Repo
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        Task<IEnumerable<Brand>> GetAllWithClothingItemsAsync();
        Task<Brand?> GetByIdWithClothingItemsAsync(int id);
    }
}
