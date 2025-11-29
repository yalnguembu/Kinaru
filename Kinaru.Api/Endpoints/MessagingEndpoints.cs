using System.Security.Claims;
using Kinaru.Api.Services.Interfaces;
using Kinaru.Shared.DTOs.Messaging;

namespace Kinaru.Api.Endpoints;

public static class MessagingEndpoints
{
    public static void MapMessagingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/messaging").WithTags("Messaging");

        group.MapPost("/", async (
            CreateMessageDto dto,
            IMessagingService service,
            HttpContext context) =>
        {
            try
            {
                var userId = GetUserIdFromClaims(context);
                var message = await service.SendMessageAsync(userId, dto);
                return Results.Created($"/api/messaging/{message.Id}", message);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .RequireAuthorization()
        .WithName("SendMessage")
        .WithOpenApi();

        group.MapGet("/conversations", async (
            IMessagingService service,
            HttpContext context) =>
        {
            try
            {
                var userId = GetUserIdFromClaims(context);
                var conversations = await service.GetConversationsAsync(userId);
                return Results.Ok(conversations);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .RequireAuthorization()
        .WithName("GetConversations")
        .WithOpenApi();

        group.MapGet("/{otherUserId:guid}", async (
            Guid otherUserId,
            IMessagingService service,
            HttpContext context) =>
        {
            try
            {
                var userId = GetUserIdFromClaims(context);
                var messages = await service.GetMessagesAsync(userId, otherUserId);
                return Results.Ok(messages);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .RequireAuthorization()
        .WithName("GetMessages")
        .WithOpenApi();

        group.MapPut("/{otherUserId:guid}/read", async (
            Guid otherUserId,
            IMessagingService service,
            HttpContext context) =>
        {
            try
            {
                var userId = GetUserIdFromClaims(context);
                await service.MarkAsReadAsync(userId, otherUserId);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .RequireAuthorization()
        .WithName("MarkAsRead")
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
