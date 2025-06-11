using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Work_with_db.Tables;

namespace Work_with_db.Configure
{
    public class SizeConfiguration : IEntityTypeConfiguration<Size>
    {
        public void Configure(EntityTypeBuilder<Size> builder)
        {
            builder.ToTable("Sizes");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name).IsRequired().HasMaxLength(10);
            
            builder.HasMany(e => e.ClothingItems)
                   .WithOne(ci => ci.Size)
                   .HasForeignKey(ci => ci.SizeId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
