using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;

namespace server.Endpoints
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/products");

            // Pobieranie wszystkich produktów
            group.MapGet("/", async (DataContext context) =>
            {
                return await context.Products.Include(p => p.ProductCategory).ToListAsync();
            });

            // Pobieranie produktu po ID
            group.MapGet("/{id}", async (int id, DataContext context) =>
            {
                return await context.Products.FindAsync(id) is Product product
                    ? Results.Ok(product)
                    : Results.NotFound("Produkt nie istnieje.");
            });

            // Dodawanie produktu
            group.MapPost("/", async (Product product, DataContext context) =>
            {
                context.Products.Add(product);
                await context.SaveChangesAsync();
                return Results.Created($"/api/products/{product.Id}", product);
            });
        }
    }
}
