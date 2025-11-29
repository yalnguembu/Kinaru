using System.Security.Claims;
using Kinaru.Api.Services.Interfaces;

namespace Kinaru.Api.Endpoints;

public static class FavoriteEndpoints
{
    public static void MapFavoriteEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/favorites").WithTags("Favorites");

        group.MapGet("/", async (
            IFavoriteService service,
            HttpContext context) =>
        {
            try
            {
                var userId = GetUserIdFromClaims(context);
                var favorites = await service.GetUserFavoritesAsync(userId);
                return Results.Ok(favorites);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .RequireAuthorization()
        .WithName("GetMyFavorites")
        .WithOpenApi();

        group.MapPost("/{propertyId:guid}", async (
            Guid propertyId,
            IFavoriteService service,
            HttpContext context) =>
        {
            try
            {
                var userId = GetUserIdFromClaims(context);
                var favorite = await service.AddFavoriteAsync(userId, propertyId);
                return Results.Created($"/api/favorites/{favorite.Id}", favorite);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .RequireAuthorization()
        .WithName("AddFavorite")
        .WithOpenApi();

        group.MapDelete("/{propertyId:guid}", async (
            Guid propertyId,
            IFavoriteService service,
            HttpContext context) =>
        {
            try
            {
                var userId = GetUserIdFromClaims(context);
                await service.RemoveFavoriteAsync(userId, propertyId);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .RequireAuthorization()
        .WithName("RemoveFavorite")
        .WithOpenApi();

        group.MapGet("/check/{propertyId:guid}", async (
            Guid propertyId,
            IFavoriteService service,
            HttpContext context) =>
        {
            try
            {
                var userId = GetUserIdFromClaims(context);
                var isFavorite = await service.IsFavoriteAsync(userId, propertyId);
                return Results.Ok(new { isFavorite });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .RequireAuthorization()
        .WithName("CheckIsFavorite")
        .WithOpenApi();
    }

    private static Guid GetUserIdFromClaims(HttpContext context)
    {
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("User ID not found in claims");
        
        return Guid.Parse(userIdClaim);
    }
}
