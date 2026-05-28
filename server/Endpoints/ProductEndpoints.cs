using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using server.Data;
using server.Models;
using server.DTOs;
using server.Mappers;
using System.Security.Claims;

namespace server.Endpoints
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/products");

            // Pobieranie wszystkich produktów (nieusuniętych) z paginacją i wyszukiwaniem
            group.MapGet("/", async (DataContext context, string? search= null, int? categoryId=null, int page =1,int pageSize=10) =>
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

                if (categoryId is not null)
                {                    
                    query = query.Where(p => p.ProductCategoryId==categoryId.Value);
                }

                var products = await query
                    .Where(p => !p.IsDeleted)
                    .Skip(skip)
                    .Take(pageSize)
                    .ToListAsync();

                var dtoList = products.Select(p => p.ToProductListDto()).ToList();
                
                return Results.Ok(new
                {
                    TotalProductsNumber = totalProductsNumber,
                    CurrentPage = page,
                    totalPages = (int)Math.Ceiling(totalProductsNumber / (double)pageSize),
                    Products = dtoList
                });
            });

            // Pobieranie produktu po ID (nieusunięte)
            group.MapGet("/{id}", async (int id, DataContext context) =>
            {
                Console.WriteLine("heeej");
                var product = await context.Products
                    .Include(p => p.ProductCategories)
                    .Include(p => p.Comments)
                    .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

                return product is not null
                    ? Results.Ok(product.ToProductDetailDto())
                    : Results.NotFound("Produkt nie istnieje.");
            });

            // Dodawanie produktu
            group.MapPost("/", async (NewProductDto dto, DataContext context, ClaimsPrincipal token) =>
            {
                var userIdString = token.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdString, out int userId))
                {
                    return Results.Unauthorized(); // Odrzucamy, jeśli coś jest nie tak z tokenem
                }

                var product = dto.FromNewProductDto();

                product.CreationDate = DateTime.Now;
                product.CreatorUserId = userId;

                if (dto.productCategoryIdList != null && dto.productCategoryIdList.Any())
                {

                    var categories = await context.ProductCategories
                        .Where(c => dto.productCategoryIdList.Contains(c.Id))
                        .ToListAsync();

                    product.ProductCategories = categories;
                }

                context.Products.Add(product);
                await context.SaveChangesAsync();
                return Results.Created($"/api/products/{product.Id}", product);
            }).RequireAuthorization();

            // Aktualizowanie produktu
            group.MapPut("/{id}", async (int id, NewProductDto dto, DataContext context) =>
            {
                var product = await context.Products.Include(p => p.ProductCategories).FirstOrDefaultAsync(p => p.Id == id);
                if (product is null || product.IsDeleted)
                    return Results.NotFound("Produkt nie istnieje.");

                // Pobierz kategorie z bazy na podstawie przekazanych ID
                var categories = new List<ProductCategory>();
                if (dto.productCategoryIdList != null && dto.productCategoryIdList.Any())
                {
                    categories = await context.ProductCategories
                        .Where(c => dto.productCategoryIdList.Contains(c.Id))
                        .ToListAsync();
                }
                
                product.Title = dto.Title;
                product.Description = dto.Description;
                product.ImageUrl = dto.ImageUrl;
                product.ProductCategories = categories;
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
            }).RequireAuthorization(policy=>policy.RequireRole("Admin"));// Jeśli ktoś ma w tokenie "User", serwer z automatu wyrzuci 403 Forbidden
        }
    }
}
