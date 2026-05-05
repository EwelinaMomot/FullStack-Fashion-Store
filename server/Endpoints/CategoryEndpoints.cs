using server.Data;
using server.Models;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace server.Endpoints
{
    public static class CategoryEndpoints
    {
        public static void MapCategoryEndpoints( this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/categories");

            group.MapPost("/add", async (DataContext context, ProductCategory category) =>
            {
                context.ProductCategories.Add(category);
                await context.SaveChangesAsync();
                return Results.Created($"/api/categories/{category.Id}", category);

            });


        }
    }
}
