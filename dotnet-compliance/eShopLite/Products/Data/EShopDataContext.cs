using DataEntities;
using Microsoft.EntityFrameworkCore;

namespace Products.Data;

public class EShopDataContext : DbContext
{
    public EShopDataContext (DbContextOptions<EShopDataContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }

    public DbSet<Product> Product { get; set; } = default!;
    public DbSet<Order> Order { get; set; } = default!;
}

public static class Extensions
{
    public static void CreateDbIfNotExists(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<EShopDataContext>();
        context.Database.EnsureCreated();
        DbInitializer.Initialize(context);
    }
}


public static class DbInitializer
{
    public static void Initialize(EShopDataContext context)
    {
        if (context.Product.Any() && context.Order.Any())
            return;

        var products = new List<Product>
        {
            new Product { Name = "Solar Powered Flashlight", Description = "A fantastic product for outdoor enthusiasts", Price = 19.99m, ImageUrl = "product1.png", Stock = 100 },
            new Product { Name = "Hiking Poles", Description = "Ideal for camping and hiking trips", Price = 24.99m, ImageUrl = "product2.png", Stock = 100 },
            new Product { Name = "Outdoor Rain Jacket", Description = "This product will keep you warm and dry in all weathers", Price = 49.99m, ImageUrl = "product3.png", Stock = 100 },
            new Product { Name = "Survival Kit", Description = "A must-have for any outdoor adventurer", Price = 99.99m, ImageUrl = "product4.png", Stock = 100 },
            new Product { Name = "Outdoor Backpack", Description = "This backpack is perfect for carrying all your outdoor essentials", Price = 39.99m, ImageUrl = "product5.png", Stock = 100 },
            new Product { Name = "Camping Cookware", Description = "This cookware set is ideal for cooking outdoors", Price = 29.99m, ImageUrl = "product6.png", Stock = 100 },
            new Product { Name = "Camping Stove", Description = "This stove is perfect for cooking outdoors", Price = 49.99m, ImageUrl = "product7.png", Stock = 100 },
            new Product { Name = "Camping Lantern", Description = "This lantern is perfect for lighting up your campsite", Price = 19.99m, ImageUrl = "product8.png", Stock = 100 },
            new Product { Name = "Camping Tent", Description = "This tent is perfect for camping trips", Price = 99.99m, ImageUrl = "product9.png", Stock = 100 },
        };

        var orders = new List<Order>
        {
            new Order { Products = new List<Product> { products[0], products[1], products[2] }, Total = 94.97m, CustomerName = "John Doe", CustomerAddress = "123 Main St, Anytown, USA" },
            new Order { Products = new List<Product> { products[3], products[4], products[5] }, Total = 169.97m, CustomerName = "Jane Doe", CustomerAddress = "456 Main St, Anytown, USA" },
            new Order { Products = new List<Product> { products[6], products[7], products[8] }, Total = 169.97m, CustomerName = "John Smith", CustomerAddress = "789 Main St, Anytown, USA" },
        };

        context.AddRange(products);
        context.AddRange(orders);

        context.SaveChanges();
    }
}