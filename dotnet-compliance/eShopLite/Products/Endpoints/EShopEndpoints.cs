using DataEntities;
using Microsoft.EntityFrameworkCore;
using Products.Data;

namespace Products.Endpoints;

public static class EShopEndpoints
{
    public static void MapProductEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Product");

        group.MapGet("/", async (EShopDataContext db) =>
        {
            return await db.Product.ToListAsync();
        })
        .WithName("GetAllProducts")
        .Produces<List<Product>>(StatusCodes.Status200OK);

        group.MapGet("/{id}", async  (int id, EShopDataContext db) =>
        {
            return await db.Product.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Product model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithName("GetProductById")
        .Produces<Product>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id}", async  (int id, Product product, EShopDataContext db) =>
        {
            var affected = await db.Product
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, product.Id)
                  .SetProperty(m => m.Name, product.Name)
                  .SetProperty(m => m.Description, product.Description)
                  .SetProperty(m => m.Price, product.Price)
                  .SetProperty(m => m.ImageUrl, product.ImageUrl)
                  .SetProperty(m => m.Stock, product.Stock)
                );

            return affected == 1 ? Results.Ok() : Results.NotFound();
        })
        .WithName("UpdateProduct")
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        group.MapPost("/", async (Product product, EShopDataContext db) =>
        {
            db.Product.Add(product);
            await db.SaveChangesAsync();
            return Results.Created($"/api/Product/{product.Id}",product);
        })
        .WithName("CreateProduct")
        .Produces<Product>(StatusCodes.Status201Created);

        group.MapDelete("/{id}", async  (int id, EShopDataContext db) =>
        {
            var affected = await db.Product
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? Results.Ok() : Results.NotFound();
        })
        .WithName("DeleteProduct")
        .Produces<Product>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        var stock = routes.MapGroup("/api/Stock");

        stock.MapGet("/{id}", async  (int id, EShopDataContext db) =>
        {
            return await db.Product.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Product model
                    ? Results.Ok(model.Stock)
                    : Results.NotFound();
        })
        .WithName("GetStockById")
        .Produces<Product>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        stock.MapPut("/{id}", async  (int id, int stockAmount, EShopDataContext db) =>
        {
            var affected = await db.Product
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Stock, stockAmount)
                );

            return affected == 1 ? Results.Ok() : Results.NotFound();
        })
        .WithName("UpdateStockById")
        .Produces<Product>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }

    public static void MapOrderEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Order");

        group.MapGet("/", async (EShopDataContext db) =>
        {
            return await db.Order.ToListAsync();
        })
        .WithName("GetAllOrder")
        .Produces<List<Order>>(StatusCodes.Status200OK);

        group.MapGet("/{id}", async  (int id, EShopDataContext db) =>
        {
            return await db.Order.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Order model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithName("GetOrderById")
        .Produces<Order>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id}", async  (int id, Order order, EShopDataContext db) =>
        {
            var affected = await db.Order
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, order.Id)
                  .SetProperty(m => m.Products, order.Products)
                  .SetProperty(m => m.Total, order.Total)
                  .SetProperty(m => m.CustomerName, order.CustomerName)
                  .SetProperty(m => m.CustomerAddress, order.CustomerAddress)
                );

            return affected == 1 ? Results.Ok() : Results.NotFound();
        })
        .WithName("UpdateOrder")
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        group.MapPost("/", async (Order order, EShopDataContext db) =>
        {
            // Ensure the Products are tracked by the DbContext
            if (order.Products != null)
            {
                for (int i = 0; i < order.Products.Count; i++)
                {
                    var product = await db.Product.FindAsync(order.Products[i].Id);
                    if (product != null)
                    {
                        // Update the stock for this product
                        product.Stock -= 1;

                        // Replace the product with the one tracked by the DbContext
                        order.Products[i] = product;
                    }
                }
            }

            db.Order.Add(order);
            await db.SaveChangesAsync();
            return Results.Created($"/api/Order/{order.Id}", order);
        })
        .WithName("CreateOrder")
        .Produces<Order>(StatusCodes.Status201Created);


        group.MapDelete("/{id}", async  (int id, EShopDataContext db) =>
        {
            var affected = await db.Order
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? Results.Ok() : Results.NotFound();
        })
        .WithName("DeleteOrder")
        .Produces<Order>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
