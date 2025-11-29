using System.Security.Claims;
using Kinaru.Api.Services.Interfaces;
using Kinaru.Shared.DTOs.Users;

namespace Kinaru.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users").WithTags("Users");

        group.MapGet("/me", async (
            IUserService service,
            HttpContext context) =>
        {
            try
            {
                var userId = GetUserIdFromClaims(context);
                var profile = await service.GetUserProfileAsync(userId);
                if (profile == null) return Results.NotFound();
                return Results.Ok(profile);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .RequireAuthorization()
        .WithName("GetMyProfile")
        .WithOpenApi();

        group.MapPut("/me", async (
            UpdateUserDto dto,
            IUserService service,
            HttpContext context) =>
        {
            try
            {
                var userId = GetUserIdFromClaims(context);
                var profile = await service.UpdateProfileAsync(userId, dto);
                return Results.Ok(profile);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .RequireAuthorization()
        .WithName("UpdateMyProfile")
        .WithOpenApi();

        group.MapPost("/me/change-password", async (
            ChangePasswordDto dto,
            IUserService service,
            HttpContext context) =>
        {
            try
            {
                var userId = GetUserIdFromClaims(context);
                await service.ChangePasswordAsync(userId, dto);
                return Results.Ok(new { message = "Password changed successfully" });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .RequireAuthorization()
        .WithName("ChangePassword")
        .WithOpenApi();

        group.MapGet("/{id:guid}", async (
            Guid id,
            IUserService service) =>
        {
            var profile = await service.GetUserProfileAsync(id);
            if (profile == null) return Results.NotFound();
            return Results.Ok(profile);
        })
        .WithName("GetUserProfile")
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
