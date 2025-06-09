using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Work_with_db.Tables;

class Program
{
    public static void Main(string[] args)
    {
        var options = new DbContextOptionsBuilder<ClothingStoreDbContext>()
            .UseSqlServer("Server=DESKTOP-TRDJNPD;Database=Clothes;Trusted_Connection=True;Encrypt=False;")
            .Options;

        using var context = new ClothingStoreDbContext(options);

        Console.WriteLine("Start seed...");
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        List<Size> sizes = new List<Size> { 
            new Size { Name = "XS" }, 
            new Size { Name = "S" }, 
            new Size { Name = "M" }, 
            new Size { Name = "L" }, 
            new Size { Name = "XL" } 
        };
        context.Sizes.AddRange(sizes);
        context.SaveChanges();

        Faker<Brand> brandFaker = new Faker<Brand>()
            .RuleFor(b => b.Name, f => f.Company.CompanyName())
            .RuleFor(b => b.Country, f =>
            {
                string country = f.Address.Country();
                if (country.Length > 10)
                {
                    return country.Substring(0, 10);
                }
                else return country;
            });


        List<Brand> brands = brandFaker.Generate(5);
        context.Brands.AddRange(brands);
        context.SaveChanges();

        Faker<User> userFaker = new Faker<User>()
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Password, f => f.Internet.Password());

        List<User> users = userFaker.Generate(10);
        context.Users.AddRange(users);
        context.SaveChanges();

        Faker<ClothingItem> clothingItemFaker = new Faker<ClothingItem>()
            .RuleFor(ci => ci.Name, f => f.Commerce.ProductName())
            .RuleFor(ci => ci.SizeId, f => f.PickRandom(sizes).Id)
            .RuleFor(ci => ci.BrandId, f => f.PickRandom(brands).Id)
            .RuleFor(ci => ci.Price, f => f.Random.Decimal(10, 200))
            .RuleFor(ci => ci.Quantity, f => f.Random.Int(1, 50));

        List<ClothingItem> clothingItems = clothingItemFaker.Generate(20);
        context.ClothingItems.AddRange(clothingItems);
        context.SaveChanges();

        Faker<Order> orderFaker = new Faker<Order>()
            .RuleFor(o => o.UserId, f => f.PickRandom(users).Id)
            .RuleFor(o => o.OrderDate, f => f.Date.Past(1))
            .RuleFor(o => o.Status, f => f.PickRandom("Pending", "Completed", "Shipped"));

        List<Order> orders = orderFaker.Generate(15);
        context.Orders.AddRange(orders);
        context.SaveChanges();

        Faker<OrderItem> orderItemFaker = new Faker<OrderItem>()
            .RuleFor(oi => oi.OrderId, f => f.PickRandom(orders).Id)
            .RuleFor(oi => oi.ClothingItemId, f => f.PickRandom(clothingItems).Id)
            .RuleFor(oi => oi.Quantity, f => f.Random.Int(1, 5));

        List<OrderItem> orderItems = orderItemFaker.Generate(30);
        context.OrderItems.AddRange(orderItems);
        context.SaveChanges();

        Console.WriteLine("Seed completed!");
    }
}