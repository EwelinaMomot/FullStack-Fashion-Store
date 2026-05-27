using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOs;
using server.Mappers;
using server.Models;

namespace server.Endpoints
{
    public static class CommentEndpoints
    {
        public static void MapCommentEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/comments");
           

            // Pobierz wszystkie komentarze danego produktu (nieusunięte)
            group.MapGet("/", async (DataContext context, int productId, int page=1,int pageSize=10) =>
            {
                if (page <= 0) page = 1;
                if (pageSize <= 0) pageSize = 10;

                int skip =(page-1)*pageSize;
                int totalCommentsNumber =await context.Comments.CountAsync(c => !c.IsDeleted);
               
                var comments = await context.Comments
                    .Where(c => !c.IsDeleted && c.ProductId==productId).Skip(skip).Take(pageSize)
                    .ToListAsync();

                var commentListDto = comments.Select(c => c.ToCommentDto()).ToList();
                return Results.Ok(new
                {
                    TotalCommentsNumber = totalCommentsNumber,
                    CurrentPage = page,
                    totalPages = (int)Math.Ceiling(totalCommentsNumber / (double)pageSize),
                    Comments = commentListDto
                });
            });

            // Pobierz komentarz po ID
            group.MapGet("/{commentId}", async (int commentId, int productId, DataContext context) =>
            {
                var comment = await context.Comments
                    .FirstOrDefaultAsync(c => c.Id == commentId && !c.IsDeleted && c.ProductId==productId);

                var commentDto = comment.ToCommentDto();
                return comment is not null
                    ? Results.Ok(commentDto)
                    : Results.NotFound("Komentarz nie istnieje.");
            });

            // Dodaj nowy komentarz
            group.MapPost("/", async (NewCommentDto commentDto, DataContext context) =>
            {
                var productExists = await context.Products.AnyAsync(p => p.Id == commentDto.ProductId);
                if (!productExists)
                {
                    return Results.BadRequest($"Produkt o ID {commentDto.ProductId} nie istnieje.");
                }

                var commentEntity = commentDto.FromNewCommentDto();
                context.Comments.Add(commentEntity);
                await context.SaveChangesAsync();
                return Results.Created($"/api/comments/{commentDto.Id}", commentDto);
            });

            // Aktualizuj komentarz (content)
            group.MapPut("/{id}", async (int id, CommentDto dto, DataContext context) =>
            {
                var comment = await context.Comments.FindAsync(id);
                if (comment is null || comment.IsDeleted)
                    return Results.NotFound("Komentarz nie istnieje.");
                comment.Description = dto.Description;
                await context.SaveChangesAsync();
                return Results.Ok(comment);
            });
            // Soft-delete komentarza
            group.MapDelete("/{id}", async (int id, DataContext context) =>
            {
                var comment = await context.Comments.FindAsync(id);
                if (comment is null || comment.IsDeleted)
                    return Results.NotFound("Komentarz nie istnieje.");

                comment.IsDeleted = true;
                await context.SaveChangesAsync();
                return Results.NoContent();
            });
        }








    }
}
