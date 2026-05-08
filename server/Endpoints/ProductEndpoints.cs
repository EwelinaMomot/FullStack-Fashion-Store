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

            // Pobieranie wszystkich produktów (nieusuniętych) z paginacją i wyszukiwaniem
            group.MapGet("/", async (DataContext context, string? search= null, int page =1,int pageSize=10) =>
            {
                if (page <= 0) page = 1;
                if (pageSize <= 0) pageSize = 10;

                int skip =(page-1)*pageSize;
                int totalProductsNumber =await context.Products.CountAsync(p => !p.IsDeleted);

                var query =context.Products.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    query = query.Where(p => p.Title.ToLower().Contains(search) || p.Description.ToLower().Contains(search));
                }

                var products = await query
                    .Include(p => p.ProductCategory)
                    .Include(p => p.Comments)
                    .Where(p => !p.IsDeleted).Skip(skip).Take(pageSize)
                    .ToListAsync();

                return Results.Ok(new
                {
                    TotalProductsNumber = totalProductsNumber,
                    CurrentPage = page,
                    totalPages = (int)Math.Ceiling(totalProductsNumber / (double)pageSize),
                    Products = products
                });
            });

            // Pobieranie produktu po ID (nieusunięte)
            group.MapGet("/{id}", async (int id, DataContext context) =>
            {
                var product = await context.Products
                    .Include(p => p.ProductCategory)
                    .Include(p => p.Comments)
                    .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

                return product is not null
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

            // Aktualizowanie produktu
            group.MapPut("/{id}", async (int id, Product dto, DataContext context) =>
            {
                var product = await context.Products.FindAsync(id);
                if (product is null || product.IsDeleted)
                    return Results.NotFound("Produkt nie istnieje.");

                // Aktualizuj dozwolone pola
                product.Title = dto.Title;
                product.Description = dto.Description;
                product.ProductCategoryId = dto.ProductCategoryId;
                product.ImageUrl = dto.ImageUrl;
                // Nie nadpisujemy CreationDate ani CreatorUserId domyślnie

                await context.SaveChangesAsync();
                return Results.Ok(product);
            });

            // Soft-delete produktu
            group.MapDelete("/{id}", async (int id, DataContext context) =>
            {
                var product = await context.Products.FindAsync(id);
                if (product is null || product.IsDeleted)
                    return Results.NotFound("Produkt nie istnieje.");

                product.IsDeleted = true;
                await context.SaveChangesAsync();
                return Results.NoContent();
            });
        }
    }
}
