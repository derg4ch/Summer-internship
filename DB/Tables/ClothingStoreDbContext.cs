using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Work_with_db.Tables
{
    public class ClothingStoreDbContext : DbContext
    {
        public ClothingStoreDbContext(DbContextOptions<ClothingStoreDbContext> options): base(options)
        {

        }

        public DbSet<Size> Sizes { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ClothingItem> ClothingItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Size>(entity =>
            {
                entity.ToTable("Sizes");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name).IsRequired().HasMaxLength(10);
                entity.HasMany(e => e.ClothingItems).WithOne(ci => ci.Size).HasForeignKey(ci => ci.SizeId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brands");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Country).IsRequired().HasMaxLength(50);

                entity.HasMany(e => e.ClothingItems).WithOne(ci => ci.Brand).HasForeignKey(ci => ci.BrandId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ClothingItem>(entity =>
            {
                entity.ToTable("ClothingItems");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.Quantity).IsRequired();

                entity.HasMany(e => e.OrderItems).WithOne(oi => oi.ClothingItem).HasForeignKey(oi => oi.ClothingItemId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Password).IsRequired();
                entity.HasMany(e => e.Orders).WithOne(o => o.User).HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.OrderDate).IsRequired();
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.HasMany(e => e.OrderItems).WithOne(oi => oi.Order).HasForeignKey(oi => oi.OrderId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItems");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity).IsRequired();
            });
        }
    }
}
