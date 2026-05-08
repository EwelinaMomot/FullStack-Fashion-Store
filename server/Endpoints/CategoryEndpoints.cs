using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;

namespace server.Endpoints
{
    public static class CategoryEndpoints
    {
        public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/categories");

            // Pobierz wszystkie kategorie (nieusunięte)
            group.MapGet("/", async (DataContext context, int page =1,int pageSize=10) =>
            {
                if (page <= 0) page = 1;
                if (pageSize <= 0) pageSize = 10;
                 int skip =(page-1)*pageSize;
                 int totalCategoriesNumber =await context.ProductCategories.CountAsync(c => !c.IsDeleted);

                var categories = await context.ProductCategories
                    .Include(c => c.Products)
                    .Where(c => !c.IsDeleted).Skip(skip).Take(pageSize)
                    .ToListAsync();

                return Results.Ok(new
                {
                    TotalCategoriesNumber = totalCategoriesNumber,
                    CurrentPage = page,
                    totalPages = (int)Math.Ceiling(totalCategoriesNumber / (double)pageSize),
                    Categories = categories
                });
            });

            // Pobierz kategorię po ID
            group.MapGet("/{id}", async (int id, DataContext context) =>
            {
                var category = await context.ProductCategories
                    .Include(c => c.Products)
                    .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

                return category is not null
                    ? Results.Ok(category)
                    : Results.NotFound("Kategoria nie istnieje.");
            });

            // Dodaj nową kategorię
            group.MapPost("/", async (ProductCategory category, DataContext context) =>
            {
                context.ProductCategories.Add(category);
                await context.SaveChangesAsync();
                return Results.Created($"/api/categories/{category.Id}", category);
            });

            // Aktualizuj kategorię (name)
            group.MapPut("/{id}", async (int id, ProductCategory dto, DataContext context) =>
            {
                var category = await context.ProductCategories.FindAsync(id);
                if (category is null || category.IsDeleted)
                    return Results.NotFound("Kategoria nie istnieje.");

                category.Name = dto.Name;
                await context.SaveChangesAsync();
                return Results.Ok(category);
            });

            // Soft-delete kategorii
            group.MapDelete("/{id}", async (int id, DataContext context) =>
            {
                var category = await context.ProductCategories.FindAsync(id);
                if (category is null || category.IsDeleted)
                    return Results.NotFound("Kategoria nie istnieje.");

                category.IsDeleted = true;
                await context.SaveChangesAsync();
                return Results.NoContent();
            });
        }
    }
}
