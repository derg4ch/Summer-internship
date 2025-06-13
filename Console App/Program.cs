using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Work_with_db.Tables;

class Program
{
    public static async Task Main(string[] args)
    {
        var services = new ServiceCollection();

        services.AddDbContext<ClothingStoreDbContext>(options =>
    options.UseNpgsql("Host=localhost;Port=5432;Database=Clothes;Username=postgres;Password=root"));



        var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ClothingStoreDbContext>();
        PasswordHasher<User> passwordHasher = new PasswordHasher<User>();

        Console.WriteLine("Start seed...");
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        List<Size> sizes = new List<Size> { new Size { Name = "XS" }, new Size { Name = "S" }, new Size { Name = "M" }, new Size { Name = "L" }, new Size { Name = "XL" }};
        context.Sizes.AddRange(sizes);
        await context.SaveChangesAsync();

        Faker<Brand> brandFaker = new Faker<Brand>()
            .RuleFor(b => b.Name, f => f.Company.CompanyName())
            .RuleFor(b => b.Country, f =>
            {
                string country = f.Address.Country();

                if(country.Length > 10)
                {
                    return country.Substring(0, 10);
                }
                else
                {
                    return country;
                }
            });

        List<Brand> brands = brandFaker.Generate(5);
        context.Brands.AddRange(brands);
        await context.SaveChangesAsync();

        List<IdentityRole<int>> roles = new List<IdentityRole<int>> { new IdentityRole<int> { Name = "Customer", NormalizedName = "CUSTOMER" }, new IdentityRole<int> { Name = "Manager", NormalizedName = "MANAGER" } };
        context.Roles.AddRange(roles);
        await context.SaveChangesAsync();

        Faker faker = new Faker();
        List<User> users = new List<User>();

        for (int i = 0; i < 10; i++)
        {
            string username = faker.Internet.UserName();
            string email = faker.Internet.Email();
            string password = "VerySecretPassword1234!";

            User user = new User
            {
                UserName = username,
                Email = email,
                NormalizedUserName = username.ToUpper(),
                NormalizedEmail = email.ToUpper(),
                PasswordHash = passwordHasher.HashPassword(null, password)
            };

            users.Add(user);
        }

        context.Users.AddRange(users);
        await context.SaveChangesAsync();

        List<IdentityUserRole<int>> userRoles = new List<IdentityUserRole<int>>();

        foreach (User user in users)
        {
            var randomRole = faker.PickRandom(roles);

            userRoles.Add(new IdentityUserRole<int>
            {
                UserId = user.Id,
                RoleId = randomRole.Id
            });
        }

        context.UserRoles.AddRange(userRoles);
        await context.SaveChangesAsync();

        Faker<ClothingItem> clothingItemFaker = new Faker<ClothingItem>()
            .RuleFor(ci => ci.Name, f => f.Commerce.ProductName())
            .RuleFor(ci => ci.SizeId, f => f.PickRandom(sizes).Id)
            .RuleFor(ci => ci.BrandId, f => f.PickRandom(brands).Id)
            .RuleFor(ci => ci.Price, f => f.Random.Decimal(10, 200))
            .RuleFor(ci => ci.Quantity, f => f.Random.Int(1, 50));

        List<ClothingItem> clothingItems = clothingItemFaker.Generate(20);
        context.ClothingItems.AddRange(clothingItems);
        await context.SaveChangesAsync();

        Faker<Order> orderFaker = new Faker<Order>()
            .RuleFor(o => o.UserId, f => f.PickRandom(users).Id)
            .RuleFor(o => o.OrderDate, f => f.Date.Past(1).ToUniversalTime())
            .RuleFor(o => o.Status, f => f.PickRandom("Pending", "Completed", "Shipped"));

        List<Order> orders = orderFaker.Generate(15);
        context.Orders.AddRange(orders);
        await context.SaveChangesAsync();

        Faker<OrderItem> orderItemFaker = new Faker<OrderItem>()
            .RuleFor(oi => oi.OrderId, f => f.PickRandom(orders).Id)
            .RuleFor(oi => oi.ClothingItemId, f => f.PickRandom(clothingItems).Id)
            .RuleFor(oi => oi.Quantity, f => f.Random.Int(1, 5));

        List<OrderItem> orderItems = orderItemFaker.Generate(30);
        context.OrderItems.AddRange(orderItems);
        await context.SaveChangesAsync();

        Console.WriteLine("Seed completed!");
    }
}