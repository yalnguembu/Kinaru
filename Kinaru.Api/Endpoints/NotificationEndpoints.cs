using System.Security.Claims;
using Kinaru.Api.Services.Interfaces;

namespace Kinaru.Api.Endpoints;

public static class NotificationEndpoints
{
    public static void MapNotificationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/notifications").WithTags("Notifications");

        group.MapGet("/", async (
            INotificationService service,
            HttpContext context) =>
        {
            try
            {
                var userId = GetUserIdFromClaims(context);
                var notifications = await service.GetUserNotificationsAsync(userId);
                return Results.Ok(notifications);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .RequireAuthorization()
        .WithName("GetMyNotifications")
        .WithOpenApi();

        group.MapPut("/{id:guid}/read", async (
            Guid id,
            INotificationService service) =>
        {
            try
            {
                await service.MarkAsReadAsync(id);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .RequireAuthorization()
        .WithName("MarkNotificationAsRead")
        .WithOpenApi();

        group.MapDelete("/{id:guid}", async (
            Guid id,
            INotificationService service) =>
        {
            try
            {
                await service.DeleteNotificationAsync(id);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .RequireAuthorization()
        .WithName("DeleteNotification")
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
