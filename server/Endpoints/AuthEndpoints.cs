using Azure.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using server.Data;
using server.DTOs;
using server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
namespace server.Endpoints
{
    public static class AuthEndpoints
    {
        private static string CreateToken(User user, IConfiguration configuration)
        {
            // 1. Tworzymy listę oświadczeń (Claims) - dane o użytkowniku zaszyte w tokenie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.UserRole.Name)
            };

            // 2. Pobieramy klucz z appsettings.json i zamieniamy go na format bajtowy
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                configuration.GetSection("Token").Value!));

            // 3. Przygotowujemy"podpis (Credentials)
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // 4. Budujemy token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1), // Token ważny 24h
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public static void MapAuthorizationEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/auth");


            group.MapPost("/register", async (DataContext context, UserDto request) =>

            {
                //check if user already exists
                if (await context.Users.AnyAsync(u => u.Username == request.Username))
                {
                    return Results.BadRequest("Użytkownik o takim loginie już istnieje");
                }

                // 2. Stwórz hash i sól
                using var hmac = new HMACSHA512();

                var user = new User
                {
                    Username = request.Username,
                    RoleId = 1, //domyślna rola to zwykły user
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)),
                    PasswordSalt = hmac.Key

                };

                context.Users.Add(user);
                await context.SaveChangesAsync();

                return Results.Ok($"Pomyślnie utworzono użytkownika {request.Username}");
            });

            group.MapPost("/login", async (DataContext context, UserDto request, IConfiguration configuration) =>
            {
                var user = await context.Users.Include(u=>u.UserRole).FirstOrDefaultAsync(u => u.Username == request.Username);

                if (user == null) { return Results.BadRequest("Użytkownik o podanym loginie nie istnieje"); }

                // 2.Weryfikacja hasła

                using var hmac = new HMACSHA512(user.PasswordSalt);

                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

                // Porównujemy każdy bajt obliczonego hasha z tym z bazy
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != user.PasswordHash[i])
                        return Results.Unauthorized();
                }

                //generujemy i zwracamy token
                string token = CreateToken(user, configuration);
                return Results.Ok(token);



            });

        }

        


    }
}
