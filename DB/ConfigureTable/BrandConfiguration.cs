using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Work_with_db.Tables;

namespace Work_with_db.Configure
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Name).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Country).IsRequired().HasMaxLength(50);

            builder.HasMany(e => e.ClothingItems)
                   .WithOne(ci => ci.Brand)
                   .HasForeignKey(ci => ci.BrandId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
