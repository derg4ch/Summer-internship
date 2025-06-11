using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Work_with_db.Tables;

namespace Work_with_db.ConfigureTable
{
    public class ClothingItemConfiguration : IEntityTypeConfiguration<ClothingItem>
    {
        public void Configure(EntityTypeBuilder<ClothingItem> builder)
        {
            builder.ToTable("ClothingItems");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);

            builder.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
            
            builder.Property(e => e.Quantity).IsRequired();
            builder.HasMany(e => e.OrderItems)
                   .WithOne(oi => oi.ClothingItem)
                   .HasForeignKey(oi => oi.ClothingItemId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
