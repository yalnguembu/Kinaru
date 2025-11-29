using System.Security.Claims;
using Kinaru.Api.Services.Interfaces;
using Kinaru.Shared.DTOs.Properties;

namespace Kinaru.Api.Endpoints;

public static class PropertyEndpoints
{
    public static void MapPropertyEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/properties").WithTags("Properties");

        group.MapGet("/", async (
            [AsParameters] PropertyFilterDto filter,
            IPropertyService service) =>
        {
            var result = await service.GetPropertiesAsync(filter);
            return Results.Ok(result);
        })
        .WithName("GetProperties")
        .WithOpenApi();

        group.MapGet("/featured", async (
            IPropertyService service,
            int count = 10) =>
        {
            var properties = await service.GetFeaturedPropertiesAsync(count);
            return Results.Ok(properties);
        })
        .WithName("GetFeaturedProperties")
        .WithOpenApi();

        group.MapGet("/user/{userId:guid}", async (
            Guid userId,
            IPropertyService service) =>
        {
            var properties = await service.GetUserPropertiesAsync(userId);
            return Results.Ok(properties);
        })
        .WithName("GetUserProperties")
        .WithOpenApi();

        group.MapGet("/{id:guid}", async (
            Guid id,
            IPropertyService service) =>
        {
            var property = await service.GetPropertyByIdAsync(id);
            if (property == null) return Results.NotFound(new { message = "Property not found" });

            await service.IncrementViewCountAsync(id);
            return Results.Ok(property);
        })
        .WithName("GetPropertyById")
        .WithOpenApi();

        group.MapPost("/", async (
            CreatePropertyDto dto,
            IPropertyService service,
            HttpContext context) =>
        {
            try
            {
                var userId = GetUserIdFromClaims(context);
                var property = await service.CreatePropertyAsync(dto, userId);
                return Results.Created($"/api/properties/{property.Id}", property);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .RequireAuthorization()
        .WithName("CreateProperty")
        .WithOpenApi();

        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdatePropertyDto dto,
            IPropertyService service,
            HttpContext context) =>
        {
            try
            {
                var userId = GetUserIdFromClaims(context);
                var property = await service.UpdatePropertyAsync(id, dto, userId);
                return Results.Ok(property);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Results.Forbid();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .RequireAuthorization()
        .WithName("UpdateProperty")
        .WithOpenApi();

        group.MapDelete("/{id:guid}", async (
            Guid id,
            IPropertyService service,
            HttpContext context) =>
        {
            try
            {
                var userId = GetUserIdFromClaims(context);
                await service.DeletePropertyAsync(id, userId);
                return Results.NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Results.Forbid();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .RequireAuthorization()
        .WithName("DeleteProperty")
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
