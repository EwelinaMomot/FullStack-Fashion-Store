using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOs;
using server.Models;
using System.Security.Cryptography;
using System.Text;

namespace server.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {

        var group = app.MapGroup("/api/users");
            

        // Pobieranie wszystkich użytkowników (żeby Admin widział listę)
        group.MapGet("/", async (DataContext context) =>
        {
            var users = await context.Users
                .Include(u => u.UserRole)
                .Select(u => new {
                    u.Id,
                    u.Username,
                    RoleName = u.UserRole.Name,
                    u.RoleId
                })
                .ToListAsync();

            return Results.Ok(users);
        }).RequireAuthorization(policy => policy.RequireRole("Admin"));

        // Zmiana roli konkretnego użytkownika
        group.MapPut("/{id}/role", async (DataContext context, int id, UpdateUserRoleDto request) =>
        {
            var user = await context.Users.FindAsync(id);

            if (user == null)
                return Results.NotFound("Nie znaleziono użytkownika.");

            // Sprawdzamy czy nowa rola istnieje w tabeli UserRoles 
            var roleExists = await context.UserRoles.AnyAsync(r => r.Id == request.RoleId);
            if (!roleExists)
                return Results.BadRequest("Wybrana rola nie istnieje.");
            if (user.RoleId == request.RoleId)
                return Results.BadRequest("Użytkownik ma już taką samą rolę.");

            user.RoleId = request.RoleId;
            await context.SaveChangesAsync();

            return Results.Ok(new { message = $"Użytkownik {user.Username} ma teraz nową rolę: {user.UserRole.Name}" });
        }).RequireAuthorization(policy => policy.RequireRole("Admin"));

        //edycja danych użytkownika
        group.MapPut("/{id}", async (DataContext context, int id, UpdateUserDto request) =>
        {
            var user = await context.Users.FindAsync(id);
            if (user == null) return Results.NotFound("Użytkownik nie istnieje.");

            // 1. Aktualizacja loginu (jeśli podano i nie jest zajęty)
            if (!string.IsNullOrEmpty(request.Username) && request.Username != user.Username)
            {
                if (await context.Users.AnyAsync(u => u.Username == request.Username))
                    return Results.BadRequest("Ta nazwa użytkownika jest już zajęta.");

                user.Username = request.Username;
            }

            // 2. Aktualizacja hasła (jeśli podano nowe)
            if (!string.IsNullOrEmpty(request.Password))
            {
                using var hmac = new HMACSHA512();
                user.PasswordSalt = hmac.Key;
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
            }

            await context.SaveChangesAsync();
            return Results.Ok("Dane użytkownika zostały pomyślnie zaktualizowane.");
        });
    }
}