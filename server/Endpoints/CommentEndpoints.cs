using Microsoft.EntityFrameworkCore;
using server.Data;

namespace server.Endpoints
{
    public static class CommentEndpoints
    {
        public static void MapCommentEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/comments");
           

            // Pobierz wszystkie komentarze (nieusunięte)
            group.MapGet("/", async (DataContext context, int page=1,int pageSize=10) =>
            {
                if (page <= 0) page = 1;
                if (pageSize <= 0) pageSize = 10;

                int skip =(page-1)*pageSize;
                int totalCommentsNumber =await context.Comments.CountAsync(c => !c.IsDeleted);

                var comments = await context.Comments
                    .Where(c => !c.IsDeleted).Skip(skip).Take(pageSize)
                    .ToListAsync();
                return Results.Ok(new
                {
                    TotalCommentsNumber = totalCommentsNumber,
                    CurrentPage = page,
                    totalPages = (int)Math.Ceiling(totalCommentsNumber / (double)pageSize),
                    Comments = comments
                });
            });
            // Pobierz komentarz po ID
            group.MapGet("/{id}", async (int id, DataContext context) =>
            {
                var comment = await context.Comments
                    .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
                return comment is not null
                    ? Results.Ok(comment)
                    : Results.NotFound("Komentarz nie istnieje.");
            });
            // Dodaj nowy komentarz
            group.MapPost("/", async (Comment comment, DataContext context) =>
            {
                context.Comments.Add(comment);
                await context.SaveChangesAsync();
                return Results.Created($"/api/comments/{comment.Id}", comment);
            });
            // Aktualizuj komentarz (content)
            group.MapPut("/{id}", async (int id, Comment dto, DataContext context) =>
            {
                var comment = await context.Comments.FindAsync(id);
                if (comment is null || comment.IsDeleted)
                    return Results.NotFound("Komentarz nie istnieje.");
                comment.Content = dto.Content;
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
