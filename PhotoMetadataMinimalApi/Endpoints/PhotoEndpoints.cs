using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoMetadataMinimalApi.Data;
using PhotoMetadataMinimalApi.Dtos;
using PhotoMetadataMinimalApi.Models;
using PhotoMetadataMinimalApi.Services;
using System.Security.Claims;

namespace PhotoMetadataMinimalApi.Endpoints;

public static class PhotoEndpoints
{
    public static void MapPhotoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/photos").RequireAuthorization();

        group.MapPost("/upload", async (
            HttpContext context,
            [FromForm] PhotoUploadDto dto,
            ApplicationDbContext db,
            MetadataExtractorService extractor) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var (dateTaken, cameraModel, gps) = extractor.Extract(dto.File);

            var photo = new Photo
            {
                FileName = dto.File.FileName,
                UserId = userId,
                DateTaken = dateTaken,
                CameraModel = cameraModel,
                GpsLocation = gps
            };

            db.Photos.Add(photo);
            await db.SaveChangesAsync();

            return Results.Ok(photo);
        })
        .DisableAntiforgery();

        group.MapGet("/", async (
            HttpContext context,
            ApplicationDbContext db) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var photos = await db.Photos
                .Where(p => p.UserId == userId)
                .ToListAsync();

            return Results.Ok(photos);
        });

        group.MapGet("/{id}", async (
            string id,
            HttpContext context,
            ApplicationDbContext db) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var photo = await db.Photos.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            return photo is not null ? Results.Ok(photo) : Results.NotFound();
        });

        group.MapDelete("/{id}", async (
            string id,
            HttpContext context,
            ApplicationDbContext db) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var photo = await db.Photos.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (photo is null)
                return Results.NotFound();

            db.Photos.Remove(photo);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}
