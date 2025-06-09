using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Work_with_db.Tables;

namespace Work_with_db.Repo
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        public BrandRepository(ClothingStoreDbContext context) : base(context)
        {

        }
    }
}
