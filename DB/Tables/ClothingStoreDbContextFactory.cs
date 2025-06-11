using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Work_with_db.Tables
{
    public class ClothingStoreDbContextFactory : IDesignTimeDbContextFactory<ClothingStoreDbContext>
    {
        public ClothingStoreDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ClothingStoreDbContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-TRDJNPD;Database=Clothes;Trusted_Connection=True;Encrypt=False;");

            return new ClothingStoreDbContext(optionsBuilder.Options);
        }
    }
}
