using Microsoft.AspNetCore.Authorization;
using PhotoMetadataMinimalApi.Data;
using PhotoMetadataMinimalApi.Dtos;
using PhotoMetadataMinimalApi.Services;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace PhotoMetadataMinimalApi.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth");

        group.MapPost("/register", async (RegisterUserDto dto, AuthService authService, TokenService tokenService) =>
        {
            var user = await authService.RegisterAsync(dto);
            return user is null
                ? Results.BadRequest("User already exists.")
                : Results.Ok(tokenService.GenerateToken(user));
        });

        group.MapPost("/login", async (LoginUserDto dto, AuthService authService, TokenService tokenService) =>
        {
            var user = await authService.LoginAsync(dto);
            return user is null
                ? Results.Unauthorized()
                : Results.Ok(tokenService.GenerateToken(user));
        });

        group.MapDelete("/delete", [Authorize] async (
            HttpContext context,
            ApplicationDbContext db) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return Results.Unauthorized();

            var user = await db.Users.FindAsync(userId);
            if (user is null) return Results.NotFound("User not found.");

            var photos = await db.Photos.Where(p => p.UserId == userId).ToListAsync();
            db.Photos.RemoveRange(photos);

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return Results.Ok("User and all their photos were deleted.");
        });
    }
}
